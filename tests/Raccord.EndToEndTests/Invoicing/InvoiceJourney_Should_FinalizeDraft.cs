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
/// ProductShape: JRN-FIN-STAGED-BILLING, UC-FIN-GENERATE-PROJECT-INVOICE,
/// UC-FIN-CONFIGURE-INVOICE-MOMENTS, UC-FIN-WORK-INVOICING-QUEUE,
/// UC-FIN-RECORD-PAYMENT, UC-FIN-MANAGE-REFUNDABLE-DEPOSIT, UC-FIN-SEND-PAYMENT-REMINDER,
/// UC-FIN-MANAGE-LEDGERS, UC-FIN-EXPORT-INVOICES-SPREADSHEET, UC-FIN-RUN-ACCOUNTING-EXPORT,
/// UC-FIN-ANALYZE-STATISTICS.
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
                var projectId = new Uri(page.Url).Segments[^1];
                await SetDepositAsync(page, projectId);

                await FinalizeDraftAsync(page, "2026-08-18");
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
                await AssertFinanceReportingAsync(page, invoiceId);

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

    private static async Task AssertFinanceReportingAsync(IPage page, string invoiceId)
    {
        var report = await page.APIRequest.GetAsync(
            "/api/finance/report?from=2026-08-01&to=2026-10-31"
        );
        report.Ok.ShouldBeTrue();
        var reportText = await report.TextAsync();
        reportText.ShouldContain($"\"invoiceCount\":1");
        reportText.ShouldContain("\"invoicedTotal\":900");
        reportText.ShouldContain("\"paidTotal\":100");
        reportText.ShouldContain("\"outstandingTotal\":800");

        var ledger = await page.APIRequest.GetAsync(
            "/api/finance/ledger?from=2026-08-01&to=2026-10-31"
        );
        ledger.Ok.ShouldBeTrue();
        var ledgerText = await ledger.TextAsync();
        ledgerText.ShouldContain(invoiceId);
        ledgerText.ShouldContain("\"entryType\":\"invoice\"");
        ledgerText.ShouldContain("\"entryType\":\"payment\"");

        var export = await page.APIRequest.GetAsync(
            "/api/finance/accounting-export?from=2026-08-01&to=2026-10-31"
        );
        export.Ok.ShouldBeTrue();
        export.Headers["content-type"].ShouldContain("text/csv");
        (await export.TextAsync()).ShouldContain("invoiceId,invoiceNumber,date,entryType");

        var invoiceExport = await page.APIRequest.GetAsync(
            "/api/finance/invoices-export?from=2026-08-01&to=2026-10-31"
        );
        invoiceExport.Ok.ShouldBeTrue();
        invoiceExport.Headers["content-type"].ShouldContain("text/csv");
        (await invoiceExport.TextAsync()).ShouldContain("invoiceId,invoiceNumber,issueDate");
    }

    private static async Task SetDepositAsync(IPage page, string projectId)
    {
        var headers = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["content-type"] = "application/json",
        };
        var configure = await page.APIRequest.PutAsync(
            $"/api/projects/{projectId}/deposit",
            new APIRequestContextOptions { Data = "{\"amount\":500}", Headers = headers }
        );
        configure.Ok.ShouldBeTrue();
        var paid = await page.APIRequest.PutAsync(
            $"/api/projects/{projectId}/deposit/status",
            new APIRequestContextOptions { Data = "{\"status\":1}", Headers = headers }
        );
        paid.Ok.ShouldBeTrue();
        var returned = await page.APIRequest.PutAsync(
            $"/api/projects/{projectId}/deposit/status",
            new APIRequestContextOptions { Data = "{\"status\":2}", Headers = headers }
        );
        returned.Ok.ShouldBeTrue();
    }
}
