using System.Globalization;
using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Raccord.EndToEndTests.Projects;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Invoicing;

/// <summary>
/// JRN-FIN-CREATE-STANDALONE-INVOICE / UC-FIN-CREATE-STANDALONE-INVOICE / UC-FIN-DUPLICATE-INVOICE.
/// Given a customer and billable project work, when an operator issues an invoice, duplicates it,
/// and creates a standalone draft, then the numbered invoice remains immutable and both drafts retain their own lines.
/// ProductShape: BC-INVOICING, JRN-FIN-STAGED-BILLING, UC-FIN-GENERATE-PROJECT-INVOICE.
/// </summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Invoicing)]
public sealed class InvoiceJourney_Should_FinalizeDraft(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_FinalizeADraftIntoAnImmutableNumberedInvoice()
    {
        await RunJourneyAsync(
            "invoice-finalize",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);

                var queue = await page.APIRequest.GetAsync(
                    "/api/invoicing/queue?from=2026-01-01&to=2026-12-31"
                );
                queue.Ok.ShouldBeTrue();
                (await queue.TextAsync()).ShouldContain($"Promo E2E {RunStamp}");

                await FinalizeDraftAsync(page);

                var invoiceId = new Uri(page.Url).Segments[^1];
                await AssertDuplicateAndStandaloneAsync(page, invoiceId);

                var englishDocument = await page.APIRequest.GetAsync(
                    $"/api/invoices/{invoiceId}/document?language=en"
                );
                englishDocument.Ok.ShouldBeTrue();
                englishDocument.Headers["content-type"].ShouldStartWith("application/pdf");
                var englishPdf = await englishDocument.BodyAsync();
                englishPdf.Length.ShouldBeGreaterThan(1000);
                System.Text.Encoding.ASCII.GetString(englishPdf[..5]).ShouldBe("%PDF-");
                var pdfText = System.Text.Encoding.ASCII.GetString(englishPdf).TrimEnd();
                pdfText.ShouldEndWith("%%EOF");

                // The invoice number comes from the domain's series sequence; asserting its presence
                // (not exact digits — journeys share one database within a run) proves the final
                // numbering survived the round trip.
                var number = await page.GetByRole(AriaRole.Heading, new() { Level = 1 })
                    .First.InnerTextAsync();
                number.Trim().ShouldNotBeNullOrWhiteSpace();
                number.TrimStart('F').ShouldNotBeNullOrEmpty();
            }
        );
    }

    /// <summary>
    /// Given a valid invoice draft, when the billing administrator leaves the issue date blank,
    /// then the browser sends no date and the issued invoice uses the organization's current date.
    /// ProductShape: BC-INVOICING, FR-INVOICE-006, UC-ISSUE-INVOICE.
    /// </summary>
    [Fact]
    public Task Journey_Should_UseOrganizationDateWhenIssueDateIsBlank() =>
        RunJourneyAsync("invoice-date-default", CompleteBlankDateJourneyAsync);

    [Fact]
    public Task Journey_Should_PreserveExplicitIssueDate() =>
        RunJourneyAsync("invoice-date-explicit", CompleteExplicitDateJourneyAsync);

    private async Task CompleteBlankDateJourneyAsync(IPage page)
    {
        await CreateCustomerAsync(page);
        await CreateProjectWithBillableLineAsync(page);
        await page.Clock.InstallAsync(new ClockInstallOptions { Time = "2099-01-01T12:00:00Z" });
        await page.GetByLabel("Select “Camera kit with lenses”").CheckAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Create draft with 1 line(s)" })
            .ClickAsync();
        await page.WaitForURLAsync(url => url.Contains("/drafts/", StringComparison.Ordinal));
        await page.GetByText("The draft is ready to be issued.").WaitForAsync();

        (await page.GetByLabel("Issue date").InputValueAsync()).ShouldBeEmpty();
        var browserDate = await page.EvaluateAsync<string>(
            "() => new Date().toISOString().slice(0, 10)"
        );
        browserDate.ShouldBe("2099-01-01");
        var finalizeRequestTask = page.WaitForRequestAsync(request =>
            string.Equals(request.Method, "POST", StringComparison.Ordinal)
            && request.Url.Contains("/finalize", StringComparison.Ordinal)
        );
        await page.GetByRole(AriaRole.Button, new() { Name = "Issue invoice" }).ClickAsync();
        var finalizeRequest = await finalizeRequestTask;
        using var requestBody = JsonDocument.Parse(finalizeRequest.PostData!);
        requestBody.RootElement.GetProperty("issueDate").ValueKind.ShouldBe(JsonValueKind.Null);

        await page.WaitForURLAsync(url => url.Contains("/invoices/", StringComparison.Ordinal));
        await page.GetByText("Issued and unalterable.").WaitForAsync();
        var invoiceId = new Uri(page.Url).Segments[^1];
        var response = await page.APIRequest.GetAsync($"/api/invoices/{invoiceId}");
        response.Status.ShouldBe(200);
        using var invoice = JsonDocument.Parse(await response.TextAsync());
        var issueDate = DateOnly.ParseExact(
            invoice.RootElement.GetProperty("issueDate").GetString()!,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture
        );
        var issuedAt = DateTimeOffset.Parse(
            invoice.RootElement.GetProperty("issuedAt").GetString()!,
            CultureInfo.InvariantCulture
        );
        var organizationZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Madrid");
        issueDate.ShouldBe(
            DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(issuedAt, organizationZone).DateTime)
        );
        issueDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture).ShouldNotBe(browserDate);
    }

    private async Task CompleteExplicitDateJourneyAsync(IPage page)
    {
        await CreateCustomerAsync(page);
        await CreateProjectWithBillableLineAsync(page);
        await page.GetByLabel("Select “Camera kit with lenses”").CheckAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Create draft with 1 line(s)" })
            .ClickAsync();
        await page.WaitForURLAsync(url => url.Contains("/drafts/", StringComparison.Ordinal));
        await page.GetByText("The draft is ready to be issued.").WaitForAsync();

        var selectedDate = OrganizationToday.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        await page.GetByLabel("Issue date").FillAsync(selectedDate);
        var finalizeRequestTask = page.WaitForRequestAsync(request =>
            string.Equals(request.Method, "POST", StringComparison.Ordinal)
            && request.Url.Contains("/finalize", StringComparison.Ordinal)
        );
        await page.GetByRole(AriaRole.Button, new() { Name = "Issue invoice" }).ClickAsync();
        var finalizeRequest = await finalizeRequestTask;
        using var requestBody = JsonDocument.Parse(finalizeRequest.PostData!);
        requestBody.RootElement.GetProperty("issueDate").GetString().ShouldBe(selectedDate);

        await page.WaitForURLAsync(url => url.Contains("/invoices/", StringComparison.Ordinal));
        await page.GetByText("Issued and unalterable.").WaitForAsync();
        var invoiceId = new Uri(page.Url).Segments[^1];
        var response = await page.APIRequest.GetAsync($"/api/invoices/{invoiceId}");
        response.Status.ShouldBe(200);
        using var invoice = JsonDocument.Parse(await response.TextAsync());
        invoice.RootElement.GetProperty("issueDate").GetString().ShouldBe(selectedDate);
    }

    private static DateOnly OrganizationToday =>
        DateOnly.FromDateTime(
            TimeZoneInfo
                .ConvertTime(
                    DateTimeOffset.UtcNow,
                    TimeZoneInfo.FindSystemTimeZoneById("Europe/Madrid")
                )
                .DateTime
        );

    private async Task AssertDuplicateAndStandaloneAsync(IPage page, string invoiceId)
    {
        var duplicate = await page.APIRequest.PostAsync($"/api/invoices/{invoiceId}/duplicate");
        duplicate.Status.ShouldBe(201);
        (await duplicate.TextAsync()).ShouldContain("Camera kit with lenses");

        var customers = await page.APIRequest.GetAsync("/api/customers");
        customers.Ok.ShouldBeTrue();
        using var customerDocument = JsonDocument.Parse(await customers.TextAsync());
        var customerId = customerDocument
            .RootElement.EnumerateArray()
            .Single(item =>
                string.Equals(
                    item.GetProperty("legalName").GetString(),
                    $"Productora E2E {RunStamp}",
                    StringComparison.Ordinal
                )
            )
            .GetProperty("id")
            .GetGuid();
        var standalone = await page.APIRequest.PostAsync(
            "/api/invoice-drafts/standalone",
            new APIRequestContextOptions
            {
                Data =
                    $"{{\"customerId\":\"{customerId}\",\"operationDate\":\"2026-09-18\",\"lines\":[{{\"description\":\"Standalone E2E service\",\"quantity\":1,\"unitPrice\":75,\"discountPercent\":0,\"vatRate\":21}}]}}",
                Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["content-type"] = "application/json",
                },
            }
        );
        standalone.Status.ShouldBe(201);
        (await standalone.TextAsync()).ShouldContain("Standalone E2E service");
    }
}
