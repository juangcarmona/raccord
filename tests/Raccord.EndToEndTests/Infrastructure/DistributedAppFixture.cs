using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using Microsoft.AspNetCore.Identity;
using Microsoft.Playwright;
using Npgsql;
using Raccord.Infrastructure.Persistence;
using Xunit;

namespace Raccord.EndToEndTests.Infrastructure;

/// <summary>
/// Boots the unmodified Aspire AppHost (Postgres, Azurite, migrations, the API and the Vite
/// frontend) once per run via <see cref="DistributedApplicationTestingBuilder"/>, hands the
/// browser the frontend resource's external URL, and owns the shared Playwright browser. Shared
/// via <see cref="E2ECollection"/>. See ADR-0017 for the strategy.
/// </summary>
public sealed class DistributedAppFixture : IAsyncLifetime
{
    // The resource names Program.cs in the Aspire host declares; these name container/project
    // resources, not connection strings, so they do not live in application config.
    private const string PostgresResourceName = "postgres";
    private const string WebResourceName = "web";
    private const string FrontendResourceName = "frontend";

    // The frontend resource runs `npm ci` under the testing builder too, and that dominates the
    // boot on a cold runner; five minutes keeps a cold CI boot from being read as a hang.
    private static readonly TimeSpan StartupTimeout = TimeSpan.FromMinutes(5);

    private DistributedApplication? app;
    private IPlaywright? playwright;
    private IBrowser? browser;

    /// <summary>The frontend (Vite) resource's external base URL — what the browser drives.</summary>
    public Uri FrontendBaseUrl { get; private set; } = null!;

    /// <summary>The web resource's external base URL — where production serves the SPA and CSP.</summary>
    public Uri WebBaseUrl { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        // The 90-minute CI hang had no visible output because the default console logger
        // buffers; these markers make every fixture stage visible in the log as it starts,
        // so the next hang names its stage instead of costing a cancellation.
        Log("create-builder: starting");
        // The testing builder has no dev cert to trust, so http endpoints are the ones the
        // browser and the health checks can actually reach without TLS ceremony. Unsecured
        // transport must be allowed before the builder composes the app model.
        Environment.SetEnvironmentVariable("ASPIRE_ALLOW_UNSECURED_TRANSPORT", "true");

        var builder =
            await DistributedApplicationTestingBuilder.CreateAsync<global::Projects.Raccord_Aspire>();
        Log("create-builder: done");

        ForceEphemeralPostgres(builder);

        Log("build+start app: starting");
        app = await builder.BuildAsync();
        await app.StartAsync();
        Log("build+start app: done");

        using var startupCts = new CancellationTokenSource(StartupTimeout);
        await app.ResourceNotifications.WaitForResourceHealthyAsync(
            WebResourceName,
            startupCts.Token
        );
        // The frontend (Vite) resource waits for web in Program.cs, so web healthy means the Vite
        // dev server is coming up; healthy here is confirmed by the first journey's navigation.
        WebBaseUrl = app.GetEndpoint(WebResourceName, "http");
        FrontendBaseUrl = app.GetEndpoint(FrontendResourceName, "http");
        Log($"web healthy; frontend at {FrontendBaseUrl}");

        // Browser install is a no-op (sub-second) when the browser is already cached; it keeps
        // the suite runnable with no manual setup beyond a Docker-capable machine.
        Log("playwright install: starting");
        var installExitCode = await Task.Run(() =>
            Microsoft.Playwright.Program.Main(["install", "chromium"])
        );
        Log($"playwright install: exit {installExitCode}");
        if (installExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Playwright browser install failed with exit code {installExitCode}."
            );
        }

        Log("launching chromium");
        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions { Headless = true }
        );
        Log("chromium up");
    }

    /// <summary>
    /// Stage markers for the CI log. Console.WriteLine alone is swallowed by xUnit's
    /// output capture when written from a collection fixture outside a test; stderr
    /// reaches the runner log unbuffered, which is what diagnosed (and keeps diagnosing)
    /// boot-stage hangs.
    /// </summary>
    private static void Log(string stage) => Console.Error.WriteLine($"[e2e-fixture] {stage}");

    /// <summary>
    /// Program.cs gives postgres a persistent data volume so `aspire run` reuses the same
    /// container across local dev sessions. That is wrong for E2E: a persistent container
    /// carries the previous run's customers and invoices into this one, risking
    /// number-sequence and unique-constraint collisions. A session-lifetime container gives
    /// every run a clean, freshly-migrated-and-seeded database.
    /// </summary>
    private static void ForceEphemeralPostgres(IDistributedApplicationTestingBuilder builder)
    {
        var postgres = (ContainerResource)
            builder.Resources.Single(resource =>
                string.Equals(resource.Name, PostgresResourceName, StringComparison.Ordinal)
            );

        // The AppHost names a persistent volume for local development. A session-lifetime
        // container alone still reuses that volume, including its old credentials and data.
        // Keep the development volume untouched and let each E2E container start with fresh data.
        foreach (
            var mount in postgres
                .Annotations.OfType<ContainerMountAnnotation>()
                .Where(mount =>
                    mount.Type == ContainerMountType.Volume
                    && string.Equals(
                        mount.Source,
                        "raccord-postgres-data",
                        StringComparison.Ordinal
                    )
                )
                .ToArray()
        )
        {
            postgres.Annotations.Remove(mount);
        }

        builder.CreateResourceBuilder(postgres).WithLifetime(ContainerLifetime.Session);
    }

    /// <summary>
    /// A fresh browser context for one journey, rooted at the frontend base URL. The locale is
    /// pinned to en-US because the SPA picks es/en from <c>navigator.language</c> and journeys
    /// assert on localized labels; leaving it to the runner's default would make the suite's
    /// meaning depend on where it runs.
    /// </summary>
    public async Task<IBrowserContext> NewContextAsync(BrowserNewContextOptions? overrides = null)
    {
        var options = new BrowserNewContextOptions
        {
            BaseURL = FrontendBaseUrl.ToString(),
            Locale = "en-US",
            IgnoreHTTPSErrors = true,
            RecordVideoDir = PlaywrightArtifacts.VisualEvidenceEnabled
                ? PlaywrightArtifacts.EvidenceDirectory
                : null,
        };
        if (overrides is not null)
        {
            options.RecordVideoDir = overrides.RecordVideoDir ?? options.RecordVideoDir;
            options.RecordVideoSize = overrides.RecordVideoSize ?? options.RecordVideoSize;
        }
        return await Browser.NewContextAsync(options);
    }

    /// <summary>Seeds a second administrator only in the ephemeral E2E database.</summary>
    public async Task<(string Email, string Password)> CreateSecondAdministratorAsync()
    {
        const string password = "E2EReviewerPassword123!";
        var email = $"reviewer-{Guid.NewGuid():N}@example.test";
        var user = CreateE2EReviewer(email);
        var connectionString = await app!.GetConnectionStringAsync("raccord");
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();
        await using (
            var insertUser = new NpgsqlCommand(
                """
                INSERT INTO workspace.users
                    (id, user_name, normalized_user_name, email, normalized_email, email_confirmed,
                     password_hash, security_stamp, concurrency_stamp, phone_number_confirmed,
                     two_factor_enabled, lockout_enabled, access_failed_count, display_name, preferred_language)
                VALUES
                    (@id, @user_name, @normalized_user_name, @email, @normalized_email, TRUE,
                     @password_hash, @security_stamp, @concurrency_stamp, FALSE, FALSE, TRUE, 0,
                     @display_name, 'en')
                """,
                connection,
                transaction
            )
        )
        {
            insertUser.Parameters.AddWithValue("id", user.Id);
            insertUser.Parameters.AddWithValue("user_name", email);
            insertUser.Parameters.AddWithValue("normalized_user_name", user.NormalizedUserName!);
            insertUser.Parameters.AddWithValue("email", email);
            insertUser.Parameters.AddWithValue("normalized_email", user.NormalizedEmail!);
            insertUser.Parameters.AddWithValue("password_hash", user.PasswordHash!);
            insertUser.Parameters.AddWithValue("security_stamp", user.SecurityStamp!);
            insertUser.Parameters.AddWithValue("concurrency_stamp", user.ConcurrencyStamp!);
            insertUser.Parameters.AddWithValue("display_name", user.DisplayName!);
            await insertUser.ExecuteNonQueryAsync();
        }
        await using (
            var assignAdmin = new NpgsqlCommand(
                """
                INSERT INTO workspace.user_roles (user_id, role_id)
                SELECT @user_id, id FROM workspace.roles WHERE normalized_name = 'ADMIN'
                """,
                connection,
                transaction
            )
        )
        {
            assignAdmin.Parameters.AddWithValue("user_id", user.Id);
            if (await assignAdmin.ExecuteNonQueryAsync() != 1)
                throw new InvalidOperationException(
                    "The Admin role was not seeded in the E2E database."
                );
        }
        await transaction.CommitAsync();
        return (email, password);
    }

    private static RaccordUser CreateE2EReviewer(string email)
    {
        var normalizedEmail = email.ToUpperInvariant();
        var user = new RaccordUser
        {
            UserName = email,
            NormalizedUserName = normalizedEmail,
            Email = email,
            NormalizedEmail = normalizedEmail,
            EmailConfirmed = true,
            DisplayName = "E2E Reviewer",
            PreferredLanguage = "en",
            SecurityStamp = Guid.NewGuid().ToString("N"),
            ConcurrencyStamp = Guid.NewGuid().ToString("N"),
        };
        user.PasswordHash = new PasswordHasher<RaccordUser>().HashPassword(
            user,
            "E2EReviewerPassword123!"
        );
        return user;
    }

    private IBrowser Browser =>
        browser ?? throw new InvalidOperationException("Fixture not initialized.");

    public async ValueTask DisposeAsync()
    {
        if (browser is not null)
        {
            await browser.DisposeAsync();
        }

        playwright?.Dispose();

        if (app is not null)
        {
            await app.DisposeAsync();
        }
    }
}
