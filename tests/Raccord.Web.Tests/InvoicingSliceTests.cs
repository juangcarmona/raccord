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
    private static readonly AddBillableLineRequest Camera = new("equipment", "Cinema camera kit", 2m, 450m, 0m, 21m);
    private static readonly AddBillableLineRequest Gaffer = new("labour", "Gaffer, 10h day", 1m, 380m, 0m, 21m);

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
    public async Task A_project_is_billed_through_draft_finalization_document_and_audit()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: [Camera, Gaffer]);

        var draft = await CreateDraftAsync(project, project.BillableLines.Select(line => line.Id).Reverse());
        Assert.Equal(["Cinema camera kit", "Gaffer, 10h day"], draft.Lines.Select(line => line.Description));
        Assert.Equal(1548.80m, draft.Totals.Total);
        Assert.Empty(draft.ValidationIssues);

        var finalize = await client.PostAsJsonAsync($"/api/invoice-drafts/{draft.Id}/finalize", new FinalizeInvoiceDraftRequest(IssueDate));
        Assert.Equal(HttpStatusCode.Created, finalize.StatusCode);
        var invoice = (await finalize.Content.ReadFromJsonAsync<IssuedInvoiceResponse>())!;
        Assert.Equal("F2026-00001", invoice.Number);
        Assert.Equal(1280.00m, invoice.Totals.TaxableBase);
        Assert.Equal(268.80m, invoice.Totals.VatAmount);
        Assert.Equal("A00000000", invoice.Recipient.TaxId);
        Assert.Equal([1, 2], invoice.Lines.Select(line => line.Position));
        Assert.Matches("^[0-9a-f]{64}$", invoice.ContentHash);

        var reloaded = await client.GetFromJsonAsync<IssuedInvoiceResponse>($"/api/invoices/{invoice.Id}");
        Assert.Equal(invoice.ContentHash, reloaded!.ContentHash);

        var document = await client.GetAsync($"/api/invoices/{invoice.Id}/document");
        Assert.Equal("application/pdf", document.Content.Headers.ContentType?.MediaType);
        Assert.StartsWith("%PDF-", System.Text.Encoding.ASCII.GetString((await document.Content.ReadAsByteArrayAsync())[..5]), StringComparison.Ordinal);

        var audit = await client.GetFromJsonAsync<List<AuditEventResponse>>($"/api/audit-events?entityId={invoice.Id}");
        Assert.Equal(["invoice.issued", "invoice.document.stored"], audit!.Select(item => item.Action));
        Assert.Equal(invoice.ContentHash, audit![0].Details["contentHash"]);

        var billed = await client.GetFromJsonAsync<ProjectResponse>($"/api/projects/{project.Id}");
        Assert.All(billed!.BillableLines, line => Assert.Equal("invoiced", line.BillingState));
        Assert.Equal(1L, await api.QueryScalarAsync<long>($"SELECT count(*) FROM compliance_records WHERE invoice_id = '{invoice.Id}' AND status = 'pending'"));
    }

    private async Task<InvoiceDraftResponse> CreateDraftAsync(ProjectResponse project, IEnumerable<Guid> lineIds)
    {
        var response = await client.PostAsJsonAsync("/api/invoice-drafts", new CreateInvoiceDraftRequest(project.Id, [.. lineIds], null));
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<InvoiceDraftResponse>())!;
    }
}
