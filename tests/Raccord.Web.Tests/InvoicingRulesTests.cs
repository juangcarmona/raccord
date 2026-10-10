using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Npgsql;
using Raccord.Contracts.Auditing;
using Raccord.Contracts.Invoicing;
using Raccord.Contracts.Projects;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class InvoicingRulesTests(PostgresContainer postgres) : IAsyncLifetime
{
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
    public async Task Finalizing_an_incomplete_draft_lists_every_issue_and_consumes_no_number()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, customerTaxId: null, Camera);
        var draft = await CreateDraftAsync(project);
        Assert.Contains(draft.ValidationIssues, issue => string.Equals(issue.Code, "recipient.tax-id-missing", StringComparison.Ordinal));

        var response = await FinalizeAsync(draft.Id, new DateOnly(2026, 5, 11));

        var problem = await ProblemAsync(response, HttpStatusCode.UnprocessableEntity);
        Assert.Equal("draft.invalid", problem.GetProperty("code").GetString());
        Assert.Contains("recipient.tax-id-missing", problem.GetProperty("issues").EnumerateArray().Select(issue => issue.GetProperty("code").GetString()));
        Assert.Equal(0L, await api.QueryScalarAsync<long>("SELECT count(*) FROM issued_invoices"));
        Assert.Equal(0L, await api.QueryScalarAsync<long>("SELECT count(*) FROM invoice_number_sequences"));
    }

    [Fact]
    public async Task A_billable_line_cannot_be_in_two_drafts()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: Camera);
        await CreateDraftAsync(project);

        var second = await client.PostAsJsonAsync("/api/invoice-drafts", new CreateInvoiceDraftRequest(project.Id, [project.BillableLines[0].Id], null));

        var problem = await ProblemAsync(second, HttpStatusCode.UnprocessableEntity);
        Assert.Equal("billing.line-not-unbilled", problem.GetProperty("code").GetString());
    }

    [Fact]
    public async Task Discarding_a_draft_or_removing_a_line_releases_the_billable_lines()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: [Camera, Gaffer]);
        var draft = await CreateDraftAsync(project);

        var afterRemoval = await (await client.DeleteAsync($"/api/invoice-drafts/{draft.Id}/lines/{draft.Lines[0].Id}")).Content.ReadFromJsonAsync<InvoiceDraftResponse>();
        Assert.Single(afterRemoval!.Lines);
        Assert.Equal(HttpStatusCode.NoContent, (await client.DeleteAsync($"/api/invoice-drafts/{draft.Id}")).StatusCode);

        var reloaded = await client.GetFromJsonAsync<ProjectResponse>($"/api/projects/{project.Id}");
        Assert.All(reloaded!.BillableLines, line => Assert.Equal("unbilled", line.BillingState));
        var audit = await client.GetFromJsonAsync<List<AuditEventResponse>>($"/api/audit-events?entityId={draft.Id}");
        Assert.Equal(["invoice-draft.created", "invoice-draft.line-removed", "invoice-draft.discarded"], audit!.Select(item => item.Action));
        await CreateDraftAsync(reloaded);
    }

    [Fact]
    public async Task An_issued_draft_cannot_be_finalized_or_discarded_again()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: Camera);
        var draft = await CreateDraftAsync(project);
        Assert.Equal(HttpStatusCode.Created, (await FinalizeAsync(draft.Id, new DateOnly(2026, 5, 11))).StatusCode);

        var again = await FinalizeAsync(draft.Id, new DateOnly(2026, 5, 11));
        var discard = await client.DeleteAsync($"/api/invoice-drafts/{draft.Id}");

        Assert.Equal("draft.not-open", (await ProblemAsync(again, HttpStatusCode.Conflict)).GetProperty("code").GetString());
        Assert.Equal("draft.not-open", (await ProblemAsync(discard, HttpStatusCode.Conflict)).GetProperty("code").GetString());
    }

    [Fact]
    public async Task Numbers_follow_issue_order_and_reject_earlier_dates()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: [Camera, Gaffer]);
        var first = await CreateDraftAsync(project, project.BillableLines[0].Id);
        var second = await CreateDraftAsync(project, project.BillableLines[1].Id);
        var issued = await (await FinalizeAsync(first.Id, new DateOnly(2026, 6, 10))).Content.ReadFromJsonAsync<IssuedInvoiceResponse>();

        var backdated = await FinalizeAsync(second.Id, new DateOnly(2026, 6, 9));
        var sameDay = await (await FinalizeAsync(second.Id, new DateOnly(2026, 6, 10))).Content.ReadFromJsonAsync<IssuedInvoiceResponse>();

        Assert.Equal("F2026-00001", issued!.Number);
        var issues = (await ProblemAsync(backdated, HttpStatusCode.UnprocessableEntity)).GetProperty("issues");
        Assert.Equal("numbering.issue-date-not-accepted", issues[0].GetProperty("code").GetString());
        Assert.Equal("F2026-00002", sameDay!.Number);
    }

    [Fact]
    public async Task The_database_rejects_changes_to_issued_invoices_and_audit_events()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: Camera);
        var draft = await CreateDraftAsync(project);
        await FinalizeAsync(draft.Id, new DateOnly(2026, 5, 11));

        var update = await Assert.ThrowsAsync<PostgresException>(() => api.ExecuteAsync("UPDATE issued_invoices SET total = 1"));
        var delete = await Assert.ThrowsAsync<PostgresException>(() => api.ExecuteAsync("DELETE FROM audit_events"));
        var truncate = await Assert.ThrowsAsync<PostgresException>(() => api.ExecuteAsync("TRUNCATE issued_invoices CASCADE"));
        var compliance = await Assert.ThrowsAsync<PostgresException>(() => api.ExecuteAsync("DELETE FROM compliance_records"));

        Assert.All([update, delete, truncate, compliance], error => Assert.Equal(PostgresErrorCodes.IntegrityConstraintViolation, error.SqlState));
    }

    [Fact]
    public async Task A_lost_document_is_regenerated_identically_from_the_issued_representation()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: Camera);
        var draft = await CreateDraftAsync(project);
        var invoice = await (await FinalizeAsync(draft.Id, new DateOnly(2026, 5, 11))).Content.ReadFromJsonAsync<IssuedInvoiceResponse>();
        var original = await client.GetByteArrayAsync($"/api/invoices/{invoice!.Id}/document");

        Directory.Delete(api.DocumentRoot, recursive: true);
        var regenerated = await client.GetByteArrayAsync($"/api/invoices/{invoice.Id}/document");

        Assert.Equal(original, regenerated);
        var audit = await client.GetFromJsonAsync<List<AuditEventResponse>>($"/api/audit-events?entityId={invoice.Id}");
        Assert.Equal("invoice.document.regenerated", audit![^1].Action);
        Assert.Equal(audit[1].Details["sha256"], audit[^1].Details["sha256"]);
    }

    [Fact]
    public async Task A_tampered_issued_invoice_is_refused_instead_of_served()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: Camera);
        var draft = await CreateDraftAsync(project);
        var invoice = await (await FinalizeAsync(draft.Id, new DateOnly(2026, 5, 11))).Content.ReadFromJsonAsync<IssuedInvoiceResponse>();
        await api.ExecuteAsync(
            "ALTER TABLE issued_invoices DISABLE TRIGGER issued_invoices_append_only;" +
            "UPDATE issued_invoices SET representation = replace(representation, '900.00', '9.00');" +
            "ALTER TABLE issued_invoices ENABLE TRIGGER issued_invoices_append_only;");

        var response = await client.GetAsync($"/api/invoices/{invoice!.Id}");

        Assert.Equal("issued-invoice.integrity-violation", (await ProblemAsync(response, HttpStatusCode.InternalServerError)).GetProperty("code").GetString());
    }

    [Fact]
    public async Task Concurrent_drafts_for_the_same_line_produce_exactly_one_draft()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: Camera);
        var request = new CreateInvoiceDraftRequest(project.Id, [project.BillableLines[0].Id], null);

        var responses = await Task.WhenAll(Enumerable.Range(0, 6).Select(_ => api.CreateClient().PostAsJsonAsync("/api/invoice-drafts", request)));

        Assert.Single(responses, response => response.StatusCode == HttpStatusCode.Created);
        Assert.All(responses.Where(response => response.StatusCode != HttpStatusCode.Created), response =>
            Assert.Contains(response.StatusCode, new[] { HttpStatusCode.Conflict, HttpStatusCode.UnprocessableEntity }));
    }

    [Fact]
    public async Task Concurrent_finalizations_never_duplicate_or_skip_numbers()
    {
        var lines = Enumerable.Range(1, 6).Select(index => Camera with { Description = $"Day {index}" }).ToArray();
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: lines);
        var drafts = new List<InvoiceDraftResponse>();
        foreach (var line in project.BillableLines)
        {
            drafts.Add(await CreateDraftAsync(project, line.Id));
        }

        await Task.WhenAll(drafts.Select(draft => api.CreateClient().PostAsJsonAsync($"/api/invoice-drafts/{draft.Id}/finalize", new FinalizeInvoiceDraftRequest(new DateOnly(2026, 5, 11)))));
        foreach (var draft in drafts)
        {
            // Losers of the race got 409 and simply retry.
            await FinalizeAsync(draft.Id, new DateOnly(2026, 5, 11));
        }

        var invoices = await client.GetFromJsonAsync<List<IssuedInvoiceSummaryResponse>>("/api/invoices");
        Assert.Equal(
            Enumerable.Range(1, 6).Select(number => $"F2026-{number:D5}"),
            invoices!.Select(invoice => invoice.Number).Order(StringComparer.Ordinal));
    }

    [Theory]
    [InlineData("rocket", "projects.billable-line-kind-invalid")]
    [InlineData("equipment", "pricing.vat-rate-out-of-range")]
    public async Task Invalid_billable_lines_are_rejected_with_rule_codes(string kind, string expectedCode)
    {
        var project = await RaccordApi.ArrangeProjectAsync(client);

        var response = await client.PostAsJsonAsync($"/api/projects/{project.Id}/billable-lines", new AddBillableLineRequest(kind, "Item", 1m, 10m, 0m, 121m));

        Assert.Equal(expectedCode, (await ProblemAsync(response, HttpStatusCode.UnprocessableEntity)).GetProperty("code").GetString());
    }

    [Fact]
    public async Task Unknown_resources_return_not_found_and_health_is_ready()
    {
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"/api/invoices/{Guid.NewGuid()}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"/api/invoice-drafts/{Guid.NewGuid()}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"/api/projects/{Guid.NewGuid()}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/api/does-not-exist")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/health/ready")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/health/live")).StatusCode);
    }

    private async Task<InvoiceDraftResponse> CreateDraftAsync(ProjectResponse project, params Guid[] lineIds)
    {
        var ids = lineIds.Length > 0 ? lineIds : [.. project.BillableLines.Select(line => line.Id)];
        var response = await client.PostAsJsonAsync("/api/invoice-drafts", new CreateInvoiceDraftRequest(project.Id, ids, null));
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<InvoiceDraftResponse>())!;
    }

    private Task<HttpResponseMessage> FinalizeAsync(Guid draftId, DateOnly issueDate) =>
        client.PostAsJsonAsync($"/api/invoice-drafts/{draftId}/finalize", new FinalizeInvoiceDraftRequest(issueDate));

    private static async Task<JsonElement> ProblemAsync(HttpResponseMessage response, HttpStatusCode expected)
    {
        Assert.Equal(expected, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }
}
