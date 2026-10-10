using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using Microsoft.Playwright;
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

    public async ValueTask InitializeAsync()
    {
        // The testing builder has no dev cert to trust, so http endpoints are the ones the
        // browser and the health checks can actually reach without TLS ceremony. Unsecured
        // transport must be allowed before the builder composes the app model.
        Environment.SetEnvironmentVariable("ASPIRE_ALLOW_UNSECURED_TRANSPORT", "true");

        var builder =
            await DistributedApplicationTestingBuilder.CreateAsync<global::Projects.Raccord_Aspire>();

        // Program.cs gives postgres a persistent data volume so `aspire run` reuses the same
        // container across local dev sessions. That is wrong for E2E: a persistent container
        // carries the previous run's customers and invoices into this one, risking
        // number-sequence and unique-constraint collisions. Force a fresh, ephemeral container
        // — and therefore a clean, freshly-migrated-and-seeded database — every run.
        var postgres = (ContainerResource)
            builder.Resources.Single(resource =>
                string.Equals(resource.Name, PostgresResourceName, StringComparison.Ordinal)
            );
        builder.CreateResourceBuilder(postgres).WithLifetime(ContainerLifetime.Session);

        app = await builder.BuildAsync();
        await app.StartAsync();

        using var startupCts = new CancellationTokenSource(StartupTimeout);
        await app.ResourceNotifications.WaitForResourceHealthyAsync(
            WebResourceName,
            startupCts.Token
        );
        // The frontend (Vite) resource waits for web in Program.cs, so web healthy means the Vite
        // dev server is coming up; healthy here is confirmed by the first journey's navigation.
        FrontendBaseUrl = app.GetEndpoint(FrontendResourceName, "http");

        // Browser install is a no-op (sub-second) when the browser is already cached; it keeps
        // the suite runnable with no manual setup beyond a Docker-capable machine.
        var installExitCode = await Task.Run(() =>
            Microsoft.Playwright.Program.Main(["install", "chromium"])
        );
        if (installExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Playwright browser install failed with exit code {installExitCode}."
            );
        }

        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions { Headless = true }
        );
    }

    /// <summary>
    /// A fresh browser context for one journey, rooted at the frontend base URL. The locale is
    /// pinned to en-US because the SPA picks es/en from <c>navigator.language</c> and journeys
    /// assert on localized labels; leaving it to the runner's default would make the suite's
    /// meaning depend on where it runs.
    /// </summary>
    public async Task<IBrowserContext> NewContextAsync()
    {
        var options = new BrowserNewContextOptions
        {
            BaseURL = FrontendBaseUrl.ToString(),
            Locale = "en-US",
        };
        return await Browser.NewContextAsync(options);
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
