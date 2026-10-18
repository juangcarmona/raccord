using System.Globalization;
using System.Text.Json;
using Microsoft.Playwright;

namespace Raccord.EndToEndTests.Infrastructure;

/// <summary>
/// The shared plumbing of every browser journey: <see cref="RunJourneyAsync"/> owns the per-test
/// browser context (English locale, trace and screenshot evidence on failure), and the domain step
/// helpers below (<see cref="CreateCustomerAsync"/>, <see cref="CreateProjectWithBillableLineAsync"/>,
/// <see cref="FinalizeDraftAsync"/>) encode the reoccurring create-customer → project → line →
/// draft → invoice path so each journey asserts its own step and reuses the rest. It carries no
/// xUnit <c>[Collection]</c> of its own — the concrete journey classes declare that.
/// </summary>
public abstract class BrowserJourneyBase(DistributedAppFixture app)
{
    protected DistributedAppFixture App { get; } = app;

    /// <summary>
    /// Runs <paramref name="journey"/> with the demo user's profile language set to English and tracing on; on failure,
    /// saves trace and screenshot artifacts under <c>playwright-artifacts/</c> and rethrows. The
    /// locale is pinned because the SPA picks es/en from <c>navigator.language</c> and journeys
    /// assert on localized labels — leaving it to the runner's default would make the suite's
    /// meaning depend on where it runs.
    /// </summary>
    protected async Task RunJourneyAsync(
        string artifactName,
        Func<IPage, Task> journey,
        bool signIn = true,
        string[]? supplementalAssets = null
    )
    {
        var evidenceMode = PlaywrightArtifacts.EvidenceModeFor(GetType());
        await using var context = await App.NewContextAsync(
            captureVideo: PlaywrightArtifacts.ShouldCaptureVideo(
                evidenceMode,
                PlaywrightArtifacts.VisualEvidenceEnabled
            )
        );
        await context.Tracing.StartAsync(
            new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true,
            }
        );
        var page = await context.NewPageAsync();
        var passed = false;
        var areas = PlaywrightArtifacts.AreasFor(GetType());

        try
        {
            EnsureAreaDeclared(areas);
            if (signIn)
            {
                await SignInAsDemoUserAsync(page);
            }
            await journey(page);
            if (ShouldCaptureVisualEvidence(evidenceMode))
            {
                await PlaywrightArtifacts.CaptureSuccessAsync(page, artifactName);
            }
            await PlaywrightArtifacts.SaveTraceAsync(context, artifactName);
            passed = true;
        }
        catch
        {
            await CaptureFailureAsync(context, page, artifactName, evidenceMode);
            throw;
        }
        finally
        {
            try
            {
                await context.CloseAsync();
                if (ShouldCaptureVisualEvidence(evidenceMode))
                {
                    await PlaywrightArtifacts.SaveVideoAsync(page, artifactName);
                }
            }
            finally
            {
                WriteJourneyResult(artifactName, passed, areas, supplementalAssets, evidenceMode);
            }
        }
    }

    private static void WriteJourneyResult(
        string artifactName,
        bool passed,
        string[] areas,
        string[]? supplementalAssets,
        string evidenceMode
    ) =>
        PlaywrightArtifacts.WriteJourneyResult(
            artifactName,
            passed ? "passed" : "failed",
            areas,
            supplementalAssets,
            evidenceMode
        );

    private static bool ShouldCaptureVisualEvidence(string evidenceMode) =>
        PlaywrightArtifacts.ShouldCaptureVideo(
            evidenceMode,
            PlaywrightArtifacts.VisualEvidenceEnabled
        );

    private static Task CaptureFailureAsync(
        IBrowserContext context,
        IPage page,
        string artifactName,
        string evidenceMode
    ) =>
        string.Equals(evidenceMode, E2ETraits.EvidenceModes.Api, StringComparison.Ordinal)
            ? PlaywrightArtifacts.CaptureApiFailureAsync(context, artifactName)
            : PlaywrightArtifacts.CaptureAsync(context, page, artifactName);

    private void EnsureAreaDeclared(string[] areas)
    {
        if (areas.Length == 0)
        {
            throw new InvalidOperationException(
                $"E2E journey '{GetType().Name}' must declare at least one bounded-context Area trait."
            );
        }
    }

    private static async Task NormalizeProfileLanguageAsync(IPage page)
    {
        var current = await page.APIRequest.GetAsync("/api/auth/me");
        if (!current.Ok)
        {
            throw new InvalidOperationException("The signed-in E2E user could not be loaded.");
        }

        using var profile = JsonDocument.Parse(await current.TextAsync());
        var displayName = profile.RootElement.GetProperty("displayName").GetString() ?? "E2E Admin";
        var normalized = await page.APIRequest.PutAsync(
            "/api/auth/me",
            new APIRequestContextOptions
            {
                Data = JsonSerializer.Serialize(new { displayName, preferredLanguage = "en" }),
                Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["content-type"] = "application/json",
                },
            }
        );
        if (!normalized.Ok)
        {
            throw new InvalidOperationException(
                "The signed-in E2E user's language could not be normalized to English."
            );
        }
    }

    private static async Task SignInAsDemoUserAsync(IPage page)
    {
        await page.GotoAsync("/sign-in");
        await page.GetByLabel("Email").FillAsync("admin@example.com");
        await page.GetByLabel("Password").FillAsync("DemoPassword123!");
        await page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
        await page.WaitForURLAsync(url => url.EndsWith("/projects", StringComparison.Ordinal));

        await NormalizeProfileLanguageAsync(page);
        await page.ReloadAsync();
        await page.GetByRole(AriaRole.Heading, new() { Name = "Projects" }).WaitForAsync();
    }

    /// <summary>
    /// Registers a customer with a unique legal name (so list assertions never collide with the
    /// demo seed or a previous journey) and the tax details an invoice needs, and lands on the
    /// customers list showing the new row. Labels match by substring on purpose: optional fields
    /// render their label as "&lt;label&gt; (optional)", so exact matching would miss them.
    /// </summary>
    protected async Task CreateCustomerAsync(IPage page)
    {
        await page.GotoAsync("/customers/new");
        await page.GetByLabel("Legal name").FillAsync($"Productora E2E {RunStamp}");
        await page.GetByLabel("Tax ID").FillAsync("B87654321");
        await page.GetByLabel("Billing e-mail").FillAsync("billing@productora-e2e.example");
        await page.GetByLabel("Street and number").FillAsync("Calle de pruebas 1");
        await page.GetByLabel("Postal code").FillAsync("28001");
        await page.GetByLabel("City").FillAsync("Madrid");
        await page.GetByLabel("Country (ISO").FillAsync("ES");
        await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();

        await page.WaitForURLAsync(url => url.EndsWith("/customers", StringComparison.Ordinal));
        await page.GetByText($"Productora E2E {RunStamp}").First.WaitForAsync();
    }

    /// <summary>
    /// Creates a project named after this run for the customer <see cref="CreateCustomerAsync"/>
    /// registered in the same page session (the project form needs a customer to bill), with one
    /// billable line, and lands on the project detail page showing the line.
    /// </summary>
    protected async Task CreateProjectWithBillableLineAsync(IPage page)
    {
        await page.GotoAsync("/projects/new");
        await page.GetByLabel("Project name").FillAsync($"Promo E2E {RunStamp}");
        // The customer select lists every customer — the one this journey created is picked by
        // its unique legal name.
        await page.GetByLabel("Customer")
            .SelectOptionAsync(new SelectOptionValue { Label = $"Productora E2E {RunStamp}" });
        await page.GetByLabel("Start").FillAsync("2026-09-14");
        await page.GetByLabel("End").FillAsync("2026-09-18");
        await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();

        await page.WaitForURLAsync(url => url.Contains("/projects/", StringComparison.Ordinal));
        await page.GetByText("This project has no billable lines.").WaitForAsync();

        await page.GetByLabel("Description").FillAsync("Camera kit with lenses");
        await page.GetByLabel("Quantity").FillAsync("2");
        await page.GetByLabel("Unit price").FillAsync("450");
        await page.GetByRole(AriaRole.Button, new() { Name = "Add billable line" }).ClickAsync();
        await page.GetByText("Camera kit with lenses").First.WaitForAsync();
    }

    /// <summary>
    /// Selects the journey's billable line, creates the draft, and finalizes it with the requested
    /// issue date — the seeded issuer profile makes the draft issueable as-is. Returns after landing on the
    /// issued invoice page. The customer select nuance: finalize validation requires an invoiceable
    /// customer (tax id + address), which <see cref="CreateCustomerAsync"/> supplies.
    /// </summary>
    protected static async Task FinalizeDraftAsync(IPage page, DateOnly? issueDate = null)
    {
        await page.GetByLabel("Select “Camera kit with lenses”").CheckAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Create draft with 1 line(s)" })
            .ClickAsync();
        await page.WaitForURLAsync(url => url.Contains("/drafts/", StringComparison.Ordinal));
        await page.GetByText("The draft is ready to be issued.").WaitForAsync();

        await page.GetByLabel("Issue date")
            .FillAsync(
                (issueDate ?? DateOnly.FromDateTime(DateTime.UtcNow)).ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture
                )
            );
        await page.GetByRole(AriaRole.Button, new() { Name = "Issue invoice" }).ClickAsync();

        await page.WaitForURLAsync(url => url.Contains("/invoices/", StringComparison.Ordinal));
        await page.GetByText("Issued and unalterable.").WaitForAsync();
    }

    /// <summary>
    /// A short per-run identifier stamped into names the journeys assert on, so journeys never
    /// collide with the demo seed or with each other even though they share one database. Cached
    /// per journey instance on purpose: it is read several times per journey (fill and assert), and
    /// a fresh ticks value each read would make the journey search for a name it never created.
    /// </summary>
    protected string RunStamp =>
        runStamp ??= DateTime.UtcNow.Ticks.ToString("D19", CultureInfo.InvariantCulture)[^8..];

    private string? runStamp;
}
