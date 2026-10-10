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
    public async Task Finalizing_an_incomplete_draft_lists_every_issue_and_consumes_no_number()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, customerTaxId: null, Camera);
        var draft = await CreateDraftAsync(project);
        draft.ValidationIssues.ShouldContain(issue =>
            string.Equals(issue.Code, "recipient.tax-id-missing", StringComparison.Ordinal)
        );

        var response = await FinalizeAsync(draft.Id, new DateOnly(2026, 5, 11));

        var problem = await ProblemAsync(response, HttpStatusCode.UnprocessableEntity);
        problem.GetProperty("code").GetString().ShouldBe("draft.invalid");
        problem
            .GetProperty("issues")
            .EnumerateArray()
            .Select(issue => issue.GetProperty("code").GetString())
            .ShouldContain("recipient.tax-id-missing");
        (await api.QueryScalarAsync<long>("SELECT count(*) FROM issued_invoices")).ShouldBe(0L);
        (
            await api.QueryScalarAsync<long>("SELECT count(*) FROM invoice_number_sequences")
        ).ShouldBe(0L);
    }

    [Fact]
    public async Task A_billable_line_cannot_be_in_two_drafts()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: Camera);
        await CreateDraftAsync(project);

        var second = await client.PostAsJsonAsync(
            "/api/invoice-drafts",
            new CreateInvoiceDraftRequest(project.Id, [project.BillableLines[0].Id], null)
        );

        var problem = await ProblemAsync(second, HttpStatusCode.UnprocessableEntity);
        problem.GetProperty("code").GetString().ShouldBe("billing.line-not-unbilled");
    }

    [Fact]
    public async Task Discarding_a_draft_or_removing_a_line_releases_the_billable_lines()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: [Camera, Gaffer]);
        var draft = await CreateDraftAsync(project);

        var afterRemoval = await (
            await client.DeleteAsync($"/api/invoice-drafts/{draft.Id}/lines/{draft.Lines[0].Id}")
        ).Content.ReadFromJsonAsync<InvoiceDraftResponse>();
        afterRemoval!.Lines.ShouldHaveSingleItem();
        (await client.DeleteAsync($"/api/invoice-drafts/{draft.Id}")).StatusCode.ShouldBe(
            HttpStatusCode.NoContent
        );

        var reloaded = await client.GetFromJsonAsync<ProjectResponse>(
            $"/api/projects/{project.Id}"
        );
        reloaded!.BillableLines.ShouldAllBe(line => line.BillingState == "unbilled");
        var audit = await client.GetFromJsonAsync<List<AuditEventResponse>>(
            $"/api/audit-events?entityId={draft.Id}"
        );
        audit!
            .Select(item => item.Action)
            .ShouldBe([
                "invoice-draft.created",
                "invoice-draft.line-removed",
                "invoice-draft.discarded",
            ]);
        await CreateDraftAsync(reloaded);
    }

    [Fact]
    public async Task An_issued_draft_cannot_be_finalized_or_discarded_again()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: Camera);
        var draft = await CreateDraftAsync(project);
        (await FinalizeAsync(draft.Id, new DateOnly(2026, 5, 11))).StatusCode.ShouldBe(
            HttpStatusCode.Created
        );

        var again = await FinalizeAsync(draft.Id, new DateOnly(2026, 5, 11));
        var discard = await client.DeleteAsync($"/api/invoice-drafts/{draft.Id}");

        (await ProblemAsync(again, HttpStatusCode.Conflict))
            .GetProperty("code")
            .GetString()
            .ShouldBe("draft.not-open");
        (await ProblemAsync(discard, HttpStatusCode.Conflict))
            .GetProperty("code")
            .GetString()
            .ShouldBe("draft.not-open");
    }

    [Fact]
    public async Task Numbers_follow_issue_order_and_reject_earlier_dates()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: [Camera, Gaffer]);
        var first = await CreateDraftAsync(project, project.BillableLines[0].Id);
        var second = await CreateDraftAsync(project, project.BillableLines[1].Id);
        var issued = await (
            await FinalizeAsync(first.Id, new DateOnly(2026, 6, 10))
        ).Content.ReadFromJsonAsync<IssuedInvoiceResponse>();

        var backdated = await FinalizeAsync(second.Id, new DateOnly(2026, 6, 9));
        var sameDay = await (
            await FinalizeAsync(second.Id, new DateOnly(2026, 6, 10))
        ).Content.ReadFromJsonAsync<IssuedInvoiceResponse>();

        issued!.Number.ShouldBe("F2026-00001");
        var issues = (
            await ProblemAsync(backdated, HttpStatusCode.UnprocessableEntity)
        ).GetProperty("issues");
        issues[0].GetProperty("code").GetString().ShouldBe("numbering.issue-date-not-accepted");
        sameDay!.Number.ShouldBe("F2026-00002");
    }

    [Fact]
    public async Task The_database_rejects_changes_to_issued_invoices_and_audit_events()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: Camera);
        var draft = await CreateDraftAsync(project);
        await FinalizeAsync(draft.Id, new DateOnly(2026, 5, 11));

        var update = await Should.ThrowAsync<PostgresException>(() =>
            api.ExecuteAsync("UPDATE issued_invoices SET total = 1")
        );
        var delete = await Should.ThrowAsync<PostgresException>(() =>
            api.ExecuteAsync("DELETE FROM audit_events")
        );
        var truncate = await Should.ThrowAsync<PostgresException>(() =>
            api.ExecuteAsync("TRUNCATE issued_invoices CASCADE")
        );
        var compliance = await Should.ThrowAsync<PostgresException>(() =>
            api.ExecuteAsync("DELETE FROM compliance_records")
        );

        foreach (var error in new[] { update, delete, truncate, compliance })
        {
            error.SqlState.ShouldBe(PostgresErrorCodes.IntegrityConstraintViolation);
        }
    }

    [Fact]
    public async Task A_lost_document_is_regenerated_identically_from_the_issued_representation()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: Camera);
        var draft = await CreateDraftAsync(project);
        var invoice = await (
            await FinalizeAsync(draft.Id, new DateOnly(2026, 5, 11))
        ).Content.ReadFromJsonAsync<IssuedInvoiceResponse>();
        var original = await client.GetByteArrayAsync($"/api/invoices/{invoice!.Id}/document");

        Directory.Delete(api.DocumentRoot, recursive: true);
        var regenerated = await client.GetByteArrayAsync($"/api/invoices/{invoice.Id}/document");

        regenerated.ShouldBe(original);
        var audit = await client.GetFromJsonAsync<List<AuditEventResponse>>(
            $"/api/audit-events?entityId={invoice.Id}"
        );
        audit![^1].Action.ShouldBe("invoice.document.regenerated");
        audit[^1].Details["sha256"].ShouldBe(audit[1].Details["sha256"]);
    }

    [Fact]
    public async Task A_tampered_issued_invoice_is_refused_instead_of_served()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: Camera);
        var draft = await CreateDraftAsync(project);
        var invoice = await (
            await FinalizeAsync(draft.Id, new DateOnly(2026, 5, 11))
        ).Content.ReadFromJsonAsync<IssuedInvoiceResponse>();
        await api.ExecuteAsync(
            "ALTER TABLE issued_invoices DISABLE TRIGGER issued_invoices_append_only;"
                + "UPDATE issued_invoices SET representation = replace(representation, '900.00', '9.00');"
                + "ALTER TABLE issued_invoices ENABLE TRIGGER issued_invoices_append_only;"
        );

        var response = await client.GetAsync($"/api/invoices/{invoice!.Id}");

        (await ProblemAsync(response, HttpStatusCode.InternalServerError))
            .GetProperty("code")
            .GetString()
            .ShouldBe("issued-invoice.integrity-violation");
    }

    [Fact]
    public async Task Concurrent_drafts_for_the_same_line_produce_exactly_one_draft()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: Camera);
        var request = new CreateInvoiceDraftRequest(
            project.Id,
            [project.BillableLines[0].Id],
            null
        );

        var responses = await Task.WhenAll(
            Enumerable
                .Range(0, 6)
                .Select(_ => api.CreateClient().PostAsJsonAsync("/api/invoice-drafts", request))
        );

        responses.Count(response => response.StatusCode == HttpStatusCode.Created).ShouldBe(1);
        foreach (
            var response in responses.Where(response =>
                response.StatusCode != HttpStatusCode.Created
            )
        )
        {
            response.StatusCode.ShouldBeOneOf(
                HttpStatusCode.Conflict,
                HttpStatusCode.UnprocessableEntity
            );
        }
    }

    [Fact]
    public async Task Concurrent_finalizations_never_duplicate_or_skip_numbers()
    {
        var lines = Enumerable
            .Range(1, 6)
            .Select(index => Camera with { Description = $"Day {index}" })
            .ToArray();
        var project = await RaccordApi.ArrangeProjectAsync(client, lines: lines);
        var drafts = new List<InvoiceDraftResponse>();
        foreach (var line in project.BillableLines)
        {
            drafts.Add(await CreateDraftAsync(project, line.Id));
        }

        await Task.WhenAll(
            drafts.Select(draft =>
                api.CreateClient()
                    .PostAsJsonAsync(
                        $"/api/invoice-drafts/{draft.Id}/finalize",
                        new FinalizeInvoiceDraftRequest(new DateOnly(2026, 5, 11))
                    )
            )
        );
        foreach (var draft in drafts)
        {
            // Losers of the race got 409 and simply retry.
            await FinalizeAsync(draft.Id, new DateOnly(2026, 5, 11));
        }

        var invoices = await client.GetFromJsonAsync<List<IssuedInvoiceSummaryResponse>>(
            "/api/invoices"
        );
        invoices!
            .Select(invoice => invoice.Number)
            .Order(StringComparer.Ordinal)
            .ShouldBe(Enumerable.Range(1, 6).Select(number => $"F2026-{number:D5}"));
    }

    [Theory]
    [InlineData("rocket", "projects.billable-line-kind-invalid")]
    [InlineData("equipment", "pricing.vat-rate-out-of-range")]
    public async Task Invalid_billable_lines_are_rejected_with_rule_codes(
        string kind,
        string expectedCode
    )
    {
        var project = await RaccordApi.ArrangeProjectAsync(client);

        var response = await client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/billable-lines",
            new AddBillableLineRequest(kind, "Item", 1m, 10m, 0m, 121m)
        );

        (await ProblemAsync(response, HttpStatusCode.UnprocessableEntity))
            .GetProperty("code")
            .GetString()
            .ShouldBe(expectedCode);
    }

    [Fact]
    public async Task Unknown_resources_return_not_found_and_health_is_ready()
    {
        (await client.GetAsync($"/api/invoices/{Guid.NewGuid()}")).StatusCode.ShouldBe(
            HttpStatusCode.NotFound
        );
        (await client.GetAsync($"/api/invoice-drafts/{Guid.NewGuid()}")).StatusCode.ShouldBe(
            HttpStatusCode.NotFound
        );
        (await client.GetAsync($"/api/projects/{Guid.NewGuid()}")).StatusCode.ShouldBe(
            HttpStatusCode.NotFound
        );
        (await client.GetAsync("/api/does-not-exist")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        (await client.GetAsync("/health/ready")).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await client.GetAsync("/health/live")).StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    private async Task<InvoiceDraftResponse> CreateDraftAsync(
        ProjectResponse project,
        params Guid[] lineIds
    )
    {
        var ids = lineIds.Length > 0 ? lineIds : [.. project.BillableLines.Select(line => line.Id)];
        var response = await client.PostAsJsonAsync(
            "/api/invoice-drafts",
            new CreateInvoiceDraftRequest(project.Id, ids, null)
        );
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        return (await response.Content.ReadFromJsonAsync<InvoiceDraftResponse>())!;
    }

    private Task<HttpResponseMessage> FinalizeAsync(Guid draftId, DateOnly issueDate) =>
        client.PostAsJsonAsync(
            $"/api/invoice-drafts/{draftId}/finalize",
            new FinalizeInvoiceDraftRequest(issueDate)
        );

    private static async Task<JsonElement> ProblemAsync(
        HttpResponseMessage response,
        HttpStatusCode expected
    )
    {
        response.StatusCode.ShouldBe(expected);
        response.Content.Headers.ContentType?.MediaType.ShouldBe("application/problem+json");
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }
}
