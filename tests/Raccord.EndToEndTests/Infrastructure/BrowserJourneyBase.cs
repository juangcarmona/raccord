using System.Globalization;
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
    /// Runs <paramref name="journey"/> on a fresh, English-locale page with tracing on; on failure,
    /// saves trace and screenshot artifacts under <c>playwright-artifacts/</c> and rethrows. The
    /// locale is pinned because the SPA picks es/en from <c>navigator.language</c> and journeys
    /// assert on localized labels — leaving it to the runner's default would make the suite's
    /// meaning depend on where it runs.
    /// </summary>
    protected async Task RunJourneyAsync(string artifactName, Func<IPage, Task> journey)
    {
        await using var context = await App.NewContextAsync();
        await context.Tracing.StartAsync(
            new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true,
            }
        );
        var page = await context.NewPageAsync();

        try
        {
            await page.GotoAsync("/sign-in");
            await page.GetByLabel("Email").FillAsync("admin@example.com");
            await page.GetByLabel("Password").FillAsync("DemoPassword123!");
            await page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
            await page.WaitForURLAsync(url => url.EndsWith("/projects", StringComparison.Ordinal));
            await journey(page);
            await PlaywrightArtifacts.SaveTraceAsync(context, artifactName);
        }
        catch
        {
            await PlaywrightArtifacts.CaptureAsync(context, page, artifactName);
            throw;
        }
        finally
        {
            try
            {
                await context.CloseAsync().WaitAsync(TimeSpan.FromSeconds(30));
            }
            catch (TimeoutException)
            {
                Console.Error.WriteLine($"[e2e] {artifactName} browser context disposal timed out");
            }
        }
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

    protected static async Task EnsureInvoiceIssuerAsync(IPage page)
    {
        var response = await page.APIRequest.PutAsync(
            "/api/organization/issuer",
            new APIRequestContextOptions
            {
                DataObject = new
                {
                    legalName = "Raccord E2E S.L.",
                    taxId = "B00000000",
                    address = new
                    {
                        street = "Calle de pruebas 1",
                        postalCode = "28001",
                        city = "Madrid",
                        province = "Madrid",
                        countryCode = "ES",
                    },
                },
            }
        );
        if (!response.Ok)
        {
            throw new InvalidOperationException(await response.TextAsync());
        }
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

        var projectId = new Uri(page.Url).Segments[^1];
        var status = await page.APIRequest.PutAsync(
            $"/api/projects/{projectId}/status",
            new APIRequestContextOptions
            {
                Data = "{\"status\":2}",
                Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["content-type"] = "application/json",
                },
            }
        );
        status.Ok.ShouldBeTrue();
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
