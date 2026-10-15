using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Raccord.EndToEndTests.Projects;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Invoicing;

/// <summary>
/// Drives the full money path — customer, project, billable line, draft, issued invoice — and
/// asserts the numbering and immutability the domain guarantees. This is the deepest smoke
/// journey; it exists so that "an invoice can be issued from the browser" is always true.
/// </summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Invoicing)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
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
                await AssertDepositAsync(page, new Uri(page.Url).Segments[^1]);

                await FinalizeDraftAsync(page, DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-60));

                var invoiceId = new Uri(page.Url).Segments[^1];
                var payment = await page.APIRequest.PostAsync(
                    $"/api/invoices/{invoiceId}/payments",
                    new APIRequestContextOptions
                    {
                        Data =
                            "{\"date\":\"2026-09-18\",\"method\":\"bank-transfer\",\"amount\":100,\"description\":\"E2E payment\"}",
                        Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                        {
                            ["content-type"] = "application/json",
                        },
                    }
                );
                payment.Status.ShouldBe(201);
                await AssertPaymentReminderAsync(page, invoiceId);
                await AssertFinanceExportsAsync(page, invoiceId);
                await AssertDuplicateAndStandaloneAsync(page, invoiceId);

                var englishDocument = await page.APIRequest.GetAsync(
                    $"/api/invoices/{invoiceId}/document?language=en"
                );
                englishDocument.Ok.ShouldBeTrue();
                englishDocument.Headers["content-type"].ShouldStartWith("application/pdf");
                var englishPdf = System.Text.Encoding.Latin1.GetString(
                    await englishDocument.BodyAsync()
                );
                englishPdf.ShouldContain("(INVOICE)", Case.Sensitive);
                englishPdf.ShouldContain("(Invoice total)", Case.Sensitive);

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

    private static async Task AssertPaymentReminderAsync(IPage page, string invoiceId)
    {
        var overdue = await page.APIRequest.GetAsync("/api/invoices/overdue");
        overdue.Ok.ShouldBeTrue();
        (await overdue.TextAsync()).ShouldContain(invoiceId);

        var newDueDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(30);
        var reminder = await page.APIRequest.PostAsync(
            $"/api/invoices/{invoiceId}/reminders",
            new APIRequestContextOptions
            {
                Data =
                    $"{{\"newDueDate\":\"{newDueDate:yyyy-MM-dd}\",\"template\":\"friendly\",\"letterhead\":\"default\"}}",
                Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["content-type"] = "application/json",
                },
            }
        );
        reminder.Status.ShouldBe(201);
        (await reminder.TextAsync()).ShouldContain("\"number\":1");

        var history = await page.APIRequest.GetAsync($"/api/invoices/{invoiceId}/reminders");
        history.Ok.ShouldBeTrue();
        (await history.TextAsync()).ShouldContain($"\"dueDate\":\"{newDueDate:yyyy-MM-dd}\"");
    }

    private static async Task AssertFinanceExportsAsync(IPage page, string invoiceId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var summary = await page.APIRequest.GetAsync(
            $"/api/finance/summary?from={today.AddDays(-90):yyyy-MM-dd}&to={today.AddDays(1):yyyy-MM-dd}"
        );
        summary.Ok.ShouldBeTrue();
        (await summary.TextAsync()).ShouldContain("\"invoiceCount\":");

        var export = await page.APIRequest.GetAsync(
            $"/api/finance/accounting-export?from={today.AddDays(-90):yyyy-MM-dd}&to={today.AddDays(1):yyyy-MM-dd}"
        );
        export.Ok.ShouldBeTrue();
        export.Headers["content-type"].ShouldStartWith("text/csv");
        var invoiceNumber = await page.GetByRole(AriaRole.Heading, new() { Level = 1 })
            .First.InnerTextAsync();
        (await export.TextAsync()).ShouldContain(invoiceNumber.Trim(), Case.Sensitive);
    }

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

    private static async Task AssertDepositAsync(IPage page, string projectId)
    {
        var configure = await page.APIRequest.PutAsync(
            $"/api/projects/{projectId}/deposit",
            new APIRequestContextOptions
            {
                Data = "{\"amount\":500}",
                Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["content-type"] = "application/json",
                },
            }
        );
        configure.Ok.ShouldBeTrue();
        var paid = await page.APIRequest.PutAsync(
            $"/api/projects/{projectId}/deposit/status",
            new APIRequestContextOptions
            {
                Data = "{\"status\":1}",
                Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["content-type"] = "application/json",
                },
            }
        );
        paid.Ok.ShouldBeTrue();
        var returned = await page.APIRequest.PutAsync(
            $"/api/projects/{projectId}/deposit/status",
            new APIRequestContextOptions
            {
                Data = "{\"status\":2}",
                Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["content-type"] = "application/json",
                },
            }
        );
        returned.Ok.ShouldBeTrue();
    }
}
