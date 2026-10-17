using System.Net;
using System.Net.Http.Json;
using Raccord.Contracts.Auditing;
using Raccord.Contracts.Common;
using Raccord.Contracts.Invoicing;
using Raccord.Contracts.Organization;
using Raccord.Contracts.Projects;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class IssuerIdentityChangesTests(PostgresContainer postgres) : IAsyncLifetime
{
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
    public async Task IssuerIdentityChange_Should_RemainPendingUntilReviewed()
    {
        var issued = await IssueInvoiceAsync();
        (
            await api.QueryScalarAsync<int>(
                "SELECT issued_invoice_count FROM workspace.issuer_profiles"
            )
        ).ShouldBe(1);
        await UpdateAddressAfterIssuanceAsync();
        var pending = await CreateIdentityChangeAsync();
        await AssertDuplicateAndSelfReviewRejectedAsync(pending.Id);
        using var reviewer = CreateReviewer();
        using var otherReviewer = CreateReviewer();
        var approved = await ApproveConcurrentlyAsync(pending.Id, reviewer, otherReviewer);
        approved.Status.ShouldBe("approved");
        approved.DecidedById.ShouldNotBe(pending.RequestedById);
        await AssertApprovedIdentityAndHistoryAsync(pending.Id, issued);
        await AssertRejectedRequestRemainsRetainedAsync(reviewer);
    }

    private async Task<IssuedInvoiceResponse> IssueInvoiceAsync()
    {
        var project = await RaccordApi.ArrangeProjectAsync(
            client,
            lines: [new AddBillableLineRequest("labour", "Camera operator", 1m, 500m, 0m, 21m)]
        );
        var draftResponse = await client.PostAsJsonAsync(
            "/api/invoice-drafts",
            new CreateInvoiceDraftRequest(
                project.Id,
                project.BillableLines.Select(line => line.Id).ToArray(),
                null
            )
        );
        draftResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
        var draft =
            await draftResponse.Content.ReadFromJsonAsync<InvoiceDraftResponse>()
            ?? throw new InvalidOperationException("Invoice draft response was empty.");
        var issueResponse = await client.PostAsJsonAsync(
            $"/api/invoice-drafts/{draft.Id}/finalize",
            new FinalizeInvoiceDraftRequest(new DateOnly(2026, 10, 17))
        );
        issueResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
        return await issueResponse.Content.ReadFromJsonAsync<IssuedInvoiceResponse>()
            ?? throw new InvalidOperationException("Issued invoice response was empty.");
    }

    private async Task UpdateAddressAfterIssuanceAsync()
    {
        var addressOnly = await client.PutAsJsonAsync(
            "/api/organization/issuer",
            new UpdateIssuerProfileRequest(
                "Fotograma Rental S.L.",
                "B00000000",
                new AddressDto("Calle Nueva 2", "28013", "Madrid", "Madrid", "ES")
            )
        );
        addressOnly.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    private async Task<IssuerIdentityChangeResponse> CreateIdentityChangeAsync()
    {
        var response = await client.PostAsJsonAsync(
            "/api/organization/issuer-identity-changes",
            new CreateIssuerIdentityChangeRequest(
                "Fotograma Producciones S.L.",
                "B12345678",
                "The company updated its registered name and tax details."
            )
        );
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var pending =
            await response.Content.ReadFromJsonAsync<IssuerIdentityChangeResponse>()
            ?? throw new InvalidOperationException("Identity change response was empty.");
        pending.Status.ShouldBe("pending");
        pending.IsRequester.ShouldBeTrue();
        pending.OldLegalName.ShouldBe("Fotograma Rental S.L.");
        return pending;
    }

    private async Task AssertDuplicateAndSelfReviewRejectedAsync(Guid requestId)
    {
        var duplicate = await client.PostAsJsonAsync(
            "/api/organization/issuer-identity-changes",
            new CreateIssuerIdentityChangeRequest("Other name", "B12345678", "Second request")
        );
        duplicate.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        var selfReview = await client.PutAsJsonAsync(
            $"/api/organization/issuer-identity-changes/{requestId}",
            new DecideIssuerIdentityChangeRequest(true)
        );
        selfReview.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }

    private HttpClient CreateReviewer()
    {
        var reviewer = api.CreateClient();
        reviewer.DefaultRequestHeaders.Add("X-Test-UserId", Guid.NewGuid().ToString());
        return reviewer;
    }

    private static async Task<IssuerIdentityChangeResponse> ApproveConcurrentlyAsync(
        Guid requestId,
        HttpClient reviewer,
        HttpClient otherReviewer
    )
    {
        var concurrent = await Task.WhenAll(
            reviewer.PutAsJsonAsync(
                $"/api/organization/issuer-identity-changes/{requestId}",
                new DecideIssuerIdentityChangeRequest(true)
            ),
            otherReviewer.PutAsJsonAsync(
                $"/api/organization/issuer-identity-changes/{requestId}",
                new DecideIssuerIdentityChangeRequest(true)
            )
        );
        concurrent.Count(response => response.StatusCode == HttpStatusCode.OK).ShouldBe(1);
        concurrent
            .Count(response =>
                response.StatusCode == HttpStatusCode.Conflict
                || response.StatusCode == HttpStatusCode.UnprocessableEntity
            )
            .ShouldBe(1);
        var approval = concurrent.Single(response => response.StatusCode == HttpStatusCode.OK);
        return await approval.Content.ReadFromJsonAsync<IssuerIdentityChangeResponse>()
            ?? throw new InvalidOperationException("Approval response was empty.");
    }

    private async Task AssertApprovedIdentityAndHistoryAsync(
        Guid requestId,
        IssuedInvoiceResponse issued
    )
    {
        var active =
            await client.GetFromJsonAsync<IssuerProfileResponse>("/api/organization/issuer")
            ?? throw new InvalidOperationException("Issuer response was empty.");
        active.LegalName.ShouldBe("Fotograma Producciones S.L.");
        active.TaxId.ShouldBe("B12345678");
        var historical =
            await client.GetFromJsonAsync<IssuedInvoiceResponse>($"/api/invoices/{issued.Id}")
            ?? throw new InvalidOperationException("Historical invoice response was empty.");
        historical.Issuer.LegalName.ShouldBe("Fotograma Rental S.L.");
        historical.Issuer.TaxId.ShouldBe("B00000000");
        historical.Issuer.Address!.Street.ShouldBe("Calle Mayor 1");
        var history =
            await client.GetFromJsonAsync<List<IssuerIdentityChangeResponse>>(
                "/api/organization/issuer-identity-changes"
            ) ?? throw new InvalidOperationException("Identity change history response was empty.");
        history.Select(change => change.Id).ShouldContain(requestId);
        var audit =
            await client.GetFromJsonAsync<List<AuditEventResponse>>(
                $"/api/audit-events?entityId={requestId}"
            ) ?? throw new InvalidOperationException("Audit events response was empty.");
        audit.Select(entry => entry.Action).ShouldContain("issuer-identity-change.requested");
        audit.Select(entry => entry.Action).ShouldContain("issuer-identity-change.approved");
        audit
            .Single(entry =>
                string.Equals(
                    entry.Action,
                    "issuer-identity-change.requested",
                    StringComparison.Ordinal
                )
            )
            .Details["reason"]
            .ShouldBe("The company updated its registered name and tax details.");
    }

    private async Task AssertRejectedRequestRemainsRetainedAsync(HttpClient reviewer)
    {
        var rejectedResponse = await client.PostAsJsonAsync(
            "/api/organization/issuer-identity-changes",
            new CreateIssuerIdentityChangeRequest(
                "Fotograma Producciones Segunda S.L.",
                "B87654321",
                "A second proposed correction."
            )
        );
        rejectedResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
        var rejectedRequest =
            await rejectedResponse.Content.ReadFromJsonAsync<IssuerIdentityChangeResponse>()
            ?? throw new InvalidOperationException("Rejected request response was empty.");
        rejectedRequest.Status.ShouldBe("pending");
        var rejection = await reviewer.PutAsJsonAsync(
            $"/api/organization/issuer-identity-changes/{rejectedRequest.Id}",
            new DecideIssuerIdentityChangeRequest(false)
        );
        rejection.StatusCode.ShouldBe(HttpStatusCode.OK);
        (
            await rejection.Content.ReadFromJsonAsync<IssuerIdentityChangeResponse>()
        )!.Status.ShouldBe("rejected");
        var unchanged =
            await client.GetFromJsonAsync<IssuerProfileResponse>("/api/organization/issuer")
            ?? throw new InvalidOperationException("Issuer response was empty.");
        unchanged!.LegalName.ShouldBe("Fotograma Producciones S.L.");
        var retained =
            await client.GetFromJsonAsync<List<IssuerIdentityChangeResponse>>(
                "/api/organization/issuer-identity-changes"
            ) ?? throw new InvalidOperationException("Identity history response was empty.");
        retained.Single(change => change.Id == rejectedRequest.Id).Status.ShouldBe("rejected");
    }

    [Fact]
    public async Task IssuerIdentityChange_Should_RequirePermissionAndReasonAfterIssuance()
    {
        await IssueInvoiceAsync();
        using var unauthorized = api.CreateClient();
        unauthorized.DefaultRequestHeaders.Add("X-Test-NoPermissions", "true");
        var forbidden = await unauthorized.PostAsJsonAsync(
            "/api/organization/issuer-identity-changes",
            new CreateIssuerIdentityChangeRequest("New name", "B12345678", "A valid reason.")
        );
        forbidden.StatusCode.ShouldBe(HttpStatusCode.Forbidden);

        var response = await client.PostAsJsonAsync(
            "/api/organization/issuer-identity-changes",
            new CreateIssuerIdentityChangeRequest("New name", "B12345678", " ")
        );
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var overlongReason = await client.PostAsJsonAsync(
            "/api/organization/issuer-identity-changes",
            new CreateIssuerIdentityChangeRequest("New name", "B12345678", new string('x', 2001))
        );
        overlongReason.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var active = await client.GetFromJsonAsync<IssuerProfileResponse>(
            "/api/organization/issuer"
        );
        active!.LegalName.ShouldBe("Fotograma Rental S.L.");
    }
}
