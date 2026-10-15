using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Raccord.Contracts.Auditing;
using Raccord.Contracts.Invoicing;
using Raccord.Contracts.Projects;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class InvoicingSliceTests(PostgresContainer postgres) : IAsyncLifetime
{
    private static readonly DateOnly IssueDate = new(2026, 5, 11);
    private static readonly AddBillableLineRequest Camera = new(
        "equipment",
        "Cinema camera kit",
        2m,
        450m,
        0m,
        21m
    );
    private static readonly AddBillableLineRequest Gaffer = new(
        "labour",
        "Gaffer, 10h day",
        1m,
        380m,
        0m,
        21m
    );

    private RaccordApi api = null!;
    private HttpClient client = null!;

    public async ValueTask InitializeAsync()
    {
        api = await RaccordApi.StartAsync(postgres);
        client = api.CreateClient();
    }

    public async ValueTask DisposeAsync()
    {
        client.Dispose();
        await api.DisposeAsync();
    }

    [Fact]
    public async Task A_project_Should_be_billed_through_draft_finalization_document_and_audit()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: [Camera, Gaffer]);

        await MarkProjectActiveAsync(project.Id);
        await AssertQueueEntryAsync(project);

        var draft = await CreateDraftAsync(
            project,
            project.BillableLines.Select(line => line.Id).Reverse()
        );
        draft
            .Lines.Select(line => line.Description)
            .ShouldBe(["Cinema camera kit", "Gaffer, 10h day"]);
        draft.Totals.Total.ShouldBe(1548.80m);
        draft.ValidationIssues.ShouldBeEmpty();

        var finalize = await client.PostAsJsonAsync(
            $"/api/invoice-drafts/{draft.Id}/finalize",
            new FinalizeInvoiceDraftRequest(IssueDate)
        );
        finalize.StatusCode.ShouldBe(HttpStatusCode.Created);
        var invoice = (await finalize.Content.ReadFromJsonAsync<IssuedInvoiceResponse>())!;
        invoice.Number.ShouldBe("F2026-00001");
        invoice.Totals.TaxableBase.ShouldBe(1280.00m);
        invoice.Totals.VatAmount.ShouldBe(268.80m);
        invoice.Recipient.TaxId.ShouldBe("A00000000");
        invoice.Lines.Select(line => line.Position).ShouldBe([1, 2]);
        invoice.ContentHash.ShouldMatch("^[0-9a-f]{64}$");

        var reloaded = await client.GetFromJsonAsync<IssuedInvoiceResponse>(
            $"/api/invoices/{invoice.Id}"
        );
        reloaded!.ContentHash.ShouldBe(invoice.ContentHash);

        await AssertPdfDocumentAsync(invoice.Id);

        await AssertEnglishDocumentAsync(invoice.Id);

        await AssertInvoiceAuditAsync(invoice.Id, invoice.ContentHash);

        var billed = await client.GetFromJsonAsync<ProjectResponse>($"/api/projects/{project.Id}");
        billed!.BillableLines.ShouldAllBe(line => line.BillingState == "invoiced");
        (
            await api.QueryScalarAsync<long>(
                $"SELECT count(*) FROM compliance.compliance_records WHERE invoice_id = '{invoice.Id}' AND status = 'pending'"
            )
        ).ShouldBe(1L);
        (
            await api.QueryScalarAsync<string>(
                $"SELECT record_hash FROM compliance.compliance_records WHERE invoice_id = '{invoice.Id}'"
            )
        )!.ShouldMatch("^[0-9a-f]{64}$");
        (
            await api.QueryScalarAsync<long>(
                $"SELECT count(*) FROM compliance.compliance_records WHERE invoice_id = '{invoice.Id}' AND previous_hash IS NULL"
            )
        ).ShouldBe(1L);
    }

    private async Task<InvoiceDraftResponse> CreateDraftAsync(
        ProjectResponse project,
        IEnumerable<Guid> lineIds
    )
    {
        var response = await client.PostAsJsonAsync(
            "/api/invoice-drafts",
            new CreateInvoiceDraftRequest(project.Id, [.. lineIds], null)
        );
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        return (await response.Content.ReadFromJsonAsync<InvoiceDraftResponse>())!;
    }

    private async Task MarkProjectActiveAsync(Guid projectId)
    {
        var response = await client.PutAsJsonAsync(
            $"/api/projects/{projectId}/status",
            new UpdateProjectStatusRequest(ProjectStatus.Active)
        );
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    private async Task AssertQueueEntryAsync(ProjectResponse project)
    {
        var queue = await client.GetFromJsonAsync<List<InvoiceQueueEntryResponse>>(
            $"/api/invoicing/queue?from={IssueDate.AddMonths(-1):yyyy-MM-dd}&to={IssueDate.AddMonths(1):yyyy-MM-dd}"
        );
        queue!.ShouldContain(entry =>
            entry.ProjectId == project.Id
            && entry.ProjectName == project.Name
            && entry.UnbilledLineCount == 2
            && entry.UnbilledNetAmount == 1280m
            && !entry.Overdue
        );
    }

    private async Task AssertEnglishDocumentAsync(Guid invoiceId)
    {
        var document = await client.GetAsync($"/api/invoices/{invoiceId}/document?language=en");
        document.Content.Headers.ContentType?.MediaType.ShouldBe("application/pdf");
        System
            .Text.Encoding.Latin1.GetString(await document.Content.ReadAsByteArrayAsync())
            .ShouldContain("(INVOICE)", Case.Sensitive);
    }

    private async Task AssertPdfDocumentAsync(Guid invoiceId)
    {
        var document = await client.GetAsync($"/api/invoices/{invoiceId}/document");
        document.Content.Headers.ContentType?.MediaType.ShouldBe("application/pdf");
        System
            .Text.Encoding.ASCII.GetString((await document.Content.ReadAsByteArrayAsync())[..5])
            .ShouldStartWith("%PDF-", Case.Sensitive);
    }

    private async Task AssertInvoiceAuditAsync(Guid invoiceId, string contentHash)
    {
        var audit = await client.GetFromJsonAsync<List<AuditEventResponse>>(
            $"/api/audit-events?entityId={invoiceId}"
        );
        audit!
            .Select(item => item.Action)
            .ShouldBe([
                "invoice.issued",
                "invoice.document.stored",
                "invoice.document.regenerated",
            ]);
        audit![0].Details["contentHash"].ShouldBe(contentHash);
    }
}
