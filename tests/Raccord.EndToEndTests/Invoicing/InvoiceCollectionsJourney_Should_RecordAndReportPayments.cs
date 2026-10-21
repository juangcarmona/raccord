using System.Globalization;
using System.Net;
using System.Text.Json;
using Microsoft.Playwright;
using Raccord.Contracts.Common;
using Raccord.Contracts.Customers;
using Raccord.Contracts.Invoicing;
using Raccord.Contracts.Projects;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Invoicing;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.EvidenceMode, E2ETraits.EvidenceModes.Api)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Invoicing)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Commercial)]
public sealed class InvoiceCollectionsJourney_Should_RecordAndReportPayments(
    DistributedAppFixture app
) : BrowserJourneyBase(app)
{
    private static readonly DateOnly IssueDate = new(2025, 5, 11);
    private static DateOnly Today => OrganizationDateToday();

    /// <summary>
    /// Given synthetic project work, when the billing administrator receives a deposit, records
    /// invoice payments and sends an overdue reminder, then payment history, balances, finance
    /// reporting, ledger entries and exports agree on the issued invoice.
    /// ProductShape: BC-INVOICING, BR-GAPLESS-NUMBERING, BR-ISSUED-INVOICE-IMMUTABLE.
    /// </summary>
    [Fact]
    public Task Journey_Should_RecordPaymentsReminderAndFinanceExports() =>
        RunJourneyAsync(
            "invoice-collections-reporting",
            async page =>
            {
                var customerId = await RegisterCustomerAsync(page);
                var projectId = await CreateProjectAsync(page, customerId);
                await AssertRefundableDepositAsync(page, projectId);
                var invoice = await IssueInvoiceAsync(page, projectId);

                var partial = await page.APIRequest.PostAsync(
                    $"/api/invoices/{invoice.Id}/payments",
                    JsonOptions(
                        new RecordInvoicePaymentRequest(
                            IssueDate,
                            "bank-transfer",
                            500m,
                            "Part payment"
                        )
                    )
                );
                partial.Status.ShouldBe((int)HttpStatusCode.Created);
                using var partialBody = JsonDocument.Parse(await partial.TextAsync());
                partialBody.RootElement.GetProperty("totalPaid").GetDecimal().ShouldBe(500m);
                partialBody.RootElement.GetProperty("outstanding").GetDecimal().ShouldBe(589m);
                partialBody
                    .RootElement.GetProperty("status")
                    .GetString()
                    .ShouldBe("partially-paid");

                await AssertOverdueReminderAsync(page, invoice.Id);

                var finalPayment = await page.APIRequest.PostAsync(
                    $"/api/invoices/{invoice.Id}/payments",
                    JsonOptions(
                        new RecordInvoicePaymentRequest(IssueDate, "card", 589m, "Final payment")
                    )
                );
                finalPayment.Status.ShouldBe((int)HttpStatusCode.Created);
                using var paymentBody = JsonDocument.Parse(await finalPayment.TextAsync());
                paymentBody.RootElement.GetProperty("totalPaid").GetDecimal().ShouldBe(1089m);
                paymentBody.RootElement.GetProperty("outstanding").GetDecimal().ShouldBe(0m);
                paymentBody.RootElement.GetProperty("status").GetString().ShouldBe("paid");

                await AssertFinanceReportAsync(page, invoice.Id, invoice.Number);
            }
        );

    private async Task<Guid> RegisterCustomerAsync(IPage page)
    {
        var response = await page.APIRequest.PostAsync(
            "/api/customers",
            JsonOptions(
                new RegisterCustomerRequest(
                    $"Collections E2E {RunStamp}",
                    "B87654321",
                    new AddressDto("Calle de pruebas 1", "28001", "Madrid", "Madrid", "ES"),
                    $"collections-{RunStamp}@example.test"
                )
            )
        );
        response.Status.ShouldBe((int)HttpStatusCode.Created);
        using var body = JsonDocument.Parse(await response.TextAsync());
        body.RootElement.GetProperty("isInvoiceable").GetBoolean().ShouldBeTrue();
        return body.RootElement.GetProperty("id").GetGuid();
    }

    private async Task<Guid> CreateProjectAsync(IPage page, Guid customerId)
    {
        var projectResponse = await page.APIRequest.PostAsync(
            "/api/projects",
            JsonOptions(
                new CreateProjectRequest(
                    $"Collections E2E {RunStamp}",
                    customerId,
                    new DateOnly(2025, 5, 4),
                    new DateOnly(2025, 5, 8)
                )
            )
        );
        projectResponse.Status.ShouldBe((int)HttpStatusCode.Created);
        using var projectBody = JsonDocument.Parse(await projectResponse.TextAsync());
        var projectId = projectBody.RootElement.GetProperty("id").GetGuid();

        var lineResponse = await page.APIRequest.PostAsync(
            $"/api/projects/{projectId}/billable-lines",
            JsonOptions(new AddBillableLineRequest("equipment", "Camera kit", 2m, 450m, 0m, 21m))
        );
        lineResponse.Status.ShouldBe((int)HttpStatusCode.Created);
        return projectId;
    }

    private static async Task AssertRefundableDepositAsync(IPage page, Guid projectId)
    {
        var configured = await page.APIRequest.PutAsync(
            $"/api/projects/{projectId}/deposit",
            JsonOptions(new SetRefundableDepositRequest(250m))
        );
        configured.Status.ShouldBe((int)HttpStatusCode.OK);

        var reloaded = await page.APIRequest.GetAsync($"/api/projects/{projectId}");
        reloaded.Status.ShouldBe((int)HttpStatusCode.OK, await reloaded.TextAsync());
        using (var projectBody = JsonDocument.Parse(await reloaded.TextAsync()))
        {
            projectBody
                .RootElement.GetProperty("refundableDepositAmount")
                .GetDecimal()
                .ShouldBe(250m);
        }

        var paid = await page.APIRequest.PutAsync(
            $"/api/projects/{projectId}/deposit/status",
            JsonBody("{\"status\":1}")
        );
        paid.Status.ShouldBe((int)HttpStatusCode.OK);
        using (var paidBody = JsonDocument.Parse(await paid.TextAsync()))
        {
            paidBody.RootElement.GetProperty("refundableDepositAmount").GetDecimal().ShouldBe(250m);
            paidBody
                .RootElement.GetProperty("depositStatus")
                .GetInt32()
                .ShouldBe((int)DepositStatus.Paid);
        }

        var returned = await page.APIRequest.PutAsync(
            $"/api/projects/{projectId}/deposit/status",
            JsonBody("{\"status\":2}")
        );
        returned.Status.ShouldBe((int)HttpStatusCode.OK);
        using var returnedBody = JsonDocument.Parse(await returned.TextAsync());
        returnedBody
            .RootElement.GetProperty("depositStatus")
            .GetInt32()
            .ShouldBe((int)DepositStatus.Returned);
        returnedBody.RootElement.GetProperty("refundableDepositAmount").GetDecimal().ShouldBe(250m);
    }

    private static async Task<(Guid Id, string Number)> IssueInvoiceAsync(
        IPage page,
        Guid projectId
    )
    {
        var project = await page.APIRequest.GetAsync($"/api/projects/{projectId}");
        project.Status.ShouldBe((int)HttpStatusCode.OK);
        using var projectBody = JsonDocument.Parse(await project.TextAsync());
        var lineId = projectBody
            .RootElement.GetProperty("billableLines")[0]
            .GetProperty("id")
            .GetGuid();

        var draftResponse = await page.APIRequest.PostAsync(
            "/api/invoice-drafts",
            JsonOptions(new CreateInvoiceDraftRequest(projectId, [lineId], null))
        );
        draftResponse.Status.ShouldBe((int)HttpStatusCode.Created);
        using var draftBody = JsonDocument.Parse(await draftResponse.TextAsync());
        var draftId = draftBody.RootElement.GetProperty("id").GetGuid();

        var finalized = await page.APIRequest.PostAsync(
            $"/api/invoice-drafts/{draftId}/finalize",
            JsonOptions(new FinalizeInvoiceDraftRequest(IssueDate))
        );
        finalized.Status.ShouldBe((int)HttpStatusCode.Created, await finalized.TextAsync());
        using var invoiceBody = JsonDocument.Parse(await finalized.TextAsync());
        return (
            invoiceBody.RootElement.GetProperty("id").GetGuid(),
            invoiceBody.RootElement.GetProperty("number").GetString()!
        );
    }

    private static async Task AssertOverdueReminderAsync(IPage page, Guid invoiceId)
    {
        var overdue = await page.APIRequest.GetAsync("/api/invoices/overdue");
        overdue.Status.ShouldBe((int)HttpStatusCode.OK);
        using (var overdueBody = JsonDocument.Parse(await overdue.TextAsync()))
        {
            overdueBody
                .RootElement.EnumerateArray()
                .ShouldContain(item =>
                    item.GetProperty("invoiceId").GetGuid() == invoiceId
                    && item.GetProperty("outstanding").GetDecimal() == 589m
                );
        }

        var dueDate = Today.AddDays(30);
        var reminder = await page.APIRequest.PostAsync(
            $"/api/invoices/{invoiceId}/reminders",
            JsonOptions(new CreateInvoiceReminderRequest(dueDate, "friendly", "default"))
        );
        reminder.Status.ShouldBe((int)HttpStatusCode.Created);
        using (var reminderBody = JsonDocument.Parse(await reminder.TextAsync()))
        {
            reminderBody.RootElement.GetProperty("number").GetInt32().ShouldBe(1);
            reminderBody
                .RootElement.GetProperty("dueDate")
                .GetString()
                .ShouldBe(dueDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        var history = await page.APIRequest.GetAsync($"/api/invoices/{invoiceId}/reminders");
        history.Status.ShouldBe((int)HttpStatusCode.OK);
        using var historyBody = JsonDocument.Parse(await history.TextAsync());
        historyBody.RootElement.GetArrayLength().ShouldBe(1);
    }

    private static async Task AssertFinanceReportAsync(
        IPage page,
        Guid invoiceId,
        string invoiceNumber
    )
    {
        var date = IssueDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var report = await page.APIRequest.GetAsync($"/api/finance/report?from={date}&to={date}");
        report.Status.ShouldBe((int)HttpStatusCode.OK);
        using (var reportBody = JsonDocument.Parse(await report.TextAsync()))
        {
            reportBody
                .RootElement.GetProperty("invoiceCount")
                .GetInt32()
                .ShouldBeGreaterThanOrEqualTo(1);
            reportBody
                .RootElement.GetProperty("paidInvoiceCount")
                .GetInt32()
                .ShouldBeGreaterThanOrEqualTo(1);
            reportBody
                .RootElement.GetProperty("invoicedTotal")
                .GetDecimal()
                .ShouldBeGreaterThanOrEqualTo(1089m);
            reportBody
                .RootElement.GetProperty("paidTotal")
                .GetDecimal()
                .ShouldBeGreaterThanOrEqualTo(1089m);
        }

        var ledger = await page.APIRequest.GetAsync($"/api/finance/ledger?from={date}&to={date}");
        ledger.Status.ShouldBe((int)HttpStatusCode.OK);
        using (var ledgerBody = JsonDocument.Parse(await ledger.TextAsync()))
        {
            var invoiceEntries = ledgerBody
                .RootElement.EnumerateArray()
                .Where(entry => entry.GetProperty("invoiceId").GetGuid() == invoiceId)
                .ToArray();
            invoiceEntries.Length.ShouldBe(3);
            invoiceEntries.ShouldContain(entry =>
                string.Equals(
                    entry.GetProperty("entryType").GetString(),
                    "invoice",
                    StringComparison.Ordinal
                )
            );
            invoiceEntries
                .Count(entry =>
                    string.Equals(
                        entry.GetProperty("entryType").GetString(),
                        "payment",
                        StringComparison.Ordinal
                    )
                )
                .ShouldBe(2);
        }

        await AssertFinanceExportsAsync(page, date, invoiceNumber);
    }

    private static async Task AssertFinanceExportsAsync(
        IPage page,
        string date,
        string invoiceNumber
    )
    {
        var accountingExport = await page.APIRequest.GetAsync(
            $"/api/finance/accounting-export?from={date}&to={date}"
        );
        accountingExport.Status.ShouldBe((int)HttpStatusCode.OK);
        accountingExport.Headers["content-type"].ShouldStartWith("text/csv");
        var accountingCsv = await accountingExport.TextAsync();
        accountingCsv.ShouldContain(
            "invoiceId,invoiceNumber,date,entryType,debit,credit,balance,currency,description"
        );
        accountingCsv.ShouldContain(invoiceNumber);

        var invoiceExport = await page.APIRequest.GetAsync(
            $"/api/finance/invoices-export?from={date}&to={date}"
        );
        invoiceExport.Status.ShouldBe((int)HttpStatusCode.OK);
        var invoiceCsv = await invoiceExport.TextAsync();
        invoiceCsv.ShouldContain("invoiceId,invoiceNumber,issueDate,customer,total,currency");
        invoiceCsv.ShouldContain(invoiceNumber);
    }

    private static APIRequestContextOptions JsonOptions<T>(T value) => new() { DataObject = value };

    private static APIRequestContextOptions JsonBody(string body) =>
        new()
        {
            Data = body,
            Headers = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["content-type"] = "application/json",
            },
        };
}
