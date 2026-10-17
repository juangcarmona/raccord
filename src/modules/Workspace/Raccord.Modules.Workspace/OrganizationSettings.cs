using Raccord.Contracts.Common;
using Raccord.Contracts.Invoicing;
using Raccord.Contracts.Organization;
using Raccord.Modules.Workspace.Contracts;
using Raccord.Modules.Workspace.Domain;
using Raccord.SharedKernel.Abstractions;
using Raccord.SharedKernel.Auditing;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Parties;

namespace Raccord.Modules.Workspace;

/// <summary>Use cases for issuer details and invoice series.</summary>
public sealed class OrganizationSettings(
    IWorkspaceStore workspace,
    IWorkspaceUnitOfWork unitOfWork,
    IAuditAppender audit,
    IClock clock,
    ICurrentUser actor
)
{
    /// <summary>Reads the issuer profile, if defined.</summary>
    public async Task<IssuerProfileResponse?> GetIssuerAsync(CancellationToken cancellationToken)
    {
        var issuer = await workspace.FindIssuerAsync(cancellationToken).ConfigureAwait(false);
        return issuer is null
            ? null
            : new IssuerProfileResponse(
                issuer.LegalName,
                issuer.TaxId,
                new AddressDto(
                    issuer.Address.Street,
                    issuer.Address.PostalCode,
                    issuer.Address.City,
                    issuer.Address.Province,
                    issuer.Address.CountryCode
                ),
                issuer.HasIssuedInvoice
            );
    }

    /// <summary>Defines or updates the issuer profile used by future invoices.</summary>
    public async Task<IssuerProfileResponse> UpdateIssuerAsync(
        UpdateIssuerProfileRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var address = ToPostalAddress(request.Address);
        if (address is null)
        {
            throw new BusinessRuleViolationException(
                "issuer.address-missing",
                "The issuer address is incomplete."
            );
        }

        var existing = await workspace.FindIssuerAsync(cancellationToken).ConfigureAwait(false);
        var issuer = existing is null
            ? IssuerProfile.Define(request.LegalName ?? string.Empty, request.TaxId, address)
            : ToDomain(existing);
        if (existing is null)
        {
            workspace.AddIssuer(ToSnapshot(issuer));
        }
        else
        {
            EnsureSingleAdminUpdateIsAllowed(existing, issuer, request);
            issuer.Update(request.LegalName ?? string.Empty, request.TaxId, address);
            workspace.UpdateIssuer(ToSnapshot(issuer, existing.IssuedInvoiceCount));
        }

        audit.Append(
            new AuditEntry(
                clock.UtcNow,
                actor.Name,
                "issuer-profile.updated",
                "issuer-profile",
                issuer.Id,
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["legalName"] = issuer.LegalName,
                    ["taxId"] = issuer.TaxId,
                }
            )
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(issuer, existing?.HasIssuedInvoice ?? false);
    }

    /// <summary>Creates a pending review for a legal-identity change after invoice issuance.</summary>
    public async Task<IssuerIdentityChangeResponse> RequestIssuerIdentityChangeAsync(
        CreateIssuerIdentityChangeRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var existing = await RequireIssuedIssuerAsync(cancellationToken).ConfigureAwait(false);
        var change = CreatePendingIdentityChange(existing, request);
        workspace.AddIssuerIdentityChange(change);
        AppendRequestAudit(change);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(change);
    }

    /// <summary>Approves or rejects a pending issuer legal-identity change.</summary>
    public async Task<IssuerIdentityChangeResponse?> DecideIssuerIdentityChangeAsync(
        Guid id,
        DecideIssuerIdentityChangeRequest decision,
        CancellationToken cancellationToken
    )
    {
        var change = await workspace
            .FindIssuerIdentityChangeAsync(id, cancellationToken)
            .ConfigureAwait(false);
        if (change is null)
            return null;
        EnsureReviewerCanDecide(change);
        var issuer =
            await workspace.FindIssuerAsync(cancellationToken).ConfigureAwait(false)
            ?? throw new BusinessRuleViolationException(
                "issuer.profile-missing",
                "The issuer profile is not configured."
            );
        if (decision.Approve)
            ApplyApprovedIdentity(change, issuer);
        var decided = change with
        {
            Status = decision.Approve ? "approved" : "rejected",
            DecidedById = actor.Id,
            DecidedByName = actor.Name,
            DecidedAt = clock.UtcNow,
        };
        workspace.UpdateIssuerIdentityChange(decided);
        AppendDecisionAudit(decided);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(decided);
    }

    private async Task<IssuerProfileSnapshot> RequireIssuedIssuerAsync(
        CancellationToken cancellationToken
    )
    {
        var existing =
            await workspace.FindIssuerAsync(cancellationToken).ConfigureAwait(false)
            ?? throw new BusinessRuleViolationException(
                "issuer.profile-missing",
                "The issuer profile is not configured."
            );
        if (!existing.HasIssuedInvoice)
            throw new BusinessRuleViolationException(
                "issuer.review-not-required",
                "Use issuer settings before an invoice has been issued."
            );
        return existing;
    }

    private IssuerIdentityChangeSnapshot CreatePendingIdentityChange(
        IssuerProfileSnapshot existing,
        CreateIssuerIdentityChangeRequest request
    )
    {
        ValidateIdentityChangeRequest(request);
        var normalizedTaxId = Raccord.SharedKernel.Parties.TaxId.Normalize(request.TaxId);
        if (
            string.Equals(existing.LegalName, request.LegalName.Trim(), StringComparison.Ordinal)
            && string.Equals(existing.TaxId, normalizedTaxId, StringComparison.Ordinal)
        )
            throw new BusinessRuleViolationException(
                "issuer.identity-unchanged",
                "The proposed legal identity matches the current identity."
            );

        var proposal = IssuerProfile.Load(
            existing.Id,
            request.LegalName,
            request.TaxId,
            existing.Address
        );
        proposal.Update(request.LegalName, request.TaxId, existing.Address);
        return new IssuerIdentityChangeSnapshot(
            Guid.CreateVersion7(),
            existing.Id,
            existing.LegalName,
            existing.TaxId,
            proposal.LegalName,
            proposal.TaxId,
            request.Reason.Trim(),
            actor.Id,
            actor.Name,
            clock.UtcNow,
            "pending",
            null,
            null,
            null
        );
    }

    private static void ValidateIdentityChangeRequest(CreateIssuerIdentityChangeRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.LegalName))
            throw new BusinessRuleViolationException(
                "issuer.legal-name-missing",
                "Issuer legal name is required."
            );
        if (string.IsNullOrWhiteSpace(request.Reason))
            throw new BusinessRuleViolationException(
                "issuer.identity-reason-required",
                "A reason is required."
            );
        if (
            request.LegalName.Trim().Length > 200
            || Raccord.SharedKernel.Parties.TaxId.Normalize(request.TaxId)?.Length > 32
            || request.Reason.Trim().Length > 2000
        )
            throw new BusinessRuleViolationException(
                "issuer.identity-input-too-long",
                "The issuer identity or review reason exceeds the supported length."
            );
    }

    private void AppendRequestAudit(IssuerIdentityChangeSnapshot change) =>
        audit.Append(
            new AuditEntry(
                clock.UtcNow,
                actor.Name,
                "issuer-identity-change.requested",
                "issuer-identity-change",
                change.Id,
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["oldLegalName"] = change.OldLegalName,
                    ["oldTaxId"] = change.OldTaxId,
                    ["proposedLegalName"] = change.ProposedLegalName,
                    ["proposedTaxId"] = change.ProposedTaxId,
                    ["reason"] = change.Reason,
                    ["requestedById"] = change.RequestedById,
                    ["requestedByName"] = change.RequestedByName,
                }
            )
        );

    private void EnsureReviewerCanDecide(IssuerIdentityChangeSnapshot change)
    {
        if (!string.Equals(change.Status, "pending", StringComparison.Ordinal))
            throw new BusinessRuleViolationException(
                "issuer.identity-request-decided",
                "This request has already been decided."
            );
        if (string.Equals(change.RequestedById, actor.Id, StringComparison.Ordinal))
            throw new BusinessRuleViolationException(
                "issuer.identity-self-review",
                "The requester cannot review their own request."
            );
    }

    private void ApplyApprovedIdentity(
        IssuerIdentityChangeSnapshot change,
        IssuerProfileSnapshot issuer
    )
    {
        if (
            !string.Equals(issuer.LegalName, change.OldLegalName, StringComparison.Ordinal)
            || !string.Equals(issuer.TaxId, change.OldTaxId, StringComparison.Ordinal)
        )
            throw new BusinessRuleViolationException(
                "issuer.identity-request-stale",
                "The issuer identity changed after this request was submitted."
            );
        var profile = ToDomain(issuer);
        profile.Update(change.ProposedLegalName, change.ProposedTaxId, issuer.Address);
        workspace.UpdateIssuer(ToSnapshot(profile, issuer.IssuedInvoiceCount));
    }

    private void AppendDecisionAudit(IssuerIdentityChangeSnapshot decision) =>
        audit.Append(
            new AuditEntry(
                clock.UtcNow,
                actor.Name,
                string.Equals(decision.Status, "approved", StringComparison.Ordinal)
                    ? "issuer-identity-change.approved"
                    : "issuer-identity-change.rejected",
                "issuer-identity-change",
                decision.Id,
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["oldLegalName"] = decision.OldLegalName,
                    ["oldTaxId"] = decision.OldTaxId,
                    ["proposedLegalName"] = decision.ProposedLegalName,
                    ["proposedTaxId"] = decision.ProposedTaxId,
                    ["reason"] = decision.Reason,
                    ["requestedById"] = decision.RequestedById,
                    ["requestedByName"] = decision.RequestedByName,
                    ["decidedById"] = decision.DecidedById!,
                    ["decidedByName"] = decision.DecidedByName!,
                    ["decision"] = decision.Status,
                }
            )
        );

    /// <summary>Lists pending and completed issuer identity reviews.</summary>
    public async Task<IReadOnlyList<IssuerIdentityChangeResponse>> ListIssuerIdentityChangesAsync(
        CancellationToken cancellationToken
    ) =>
        (await workspace.ListIssuerIdentityChangesAsync(cancellationToken).ConfigureAwait(false))
            .Select(ToResponse)
            .ToArray();

    /// <summary>Lists invoice series.</summary>
    public async Task<IReadOnlyList<InvoiceSeriesResponse>> ListSeriesAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (await workspace.ListAsync(cancellationToken).ConfigureAwait(false)).Select(
                series => new InvoiceSeriesResponse(
                    series.Code,
                    series.Description,
                    series.IsDefault
                )
            ),
        ];

    private static IssuerProfile ToDomain(IssuerProfileSnapshot issuer) =>
        IssuerProfile.Load(issuer.Id, issuer.LegalName, issuer.TaxId, issuer.Address);

    private static PostalAddress? ToPostalAddress(AddressDto? address) =>
        address is null
            ? null
            : new PostalAddress(
                address.Street,
                address.PostalCode,
                address.City,
                address.Province,
                address.CountryCode
            );

    private static void EnsureSingleAdminUpdateIsAllowed(
        IssuerProfileSnapshot existing,
        IssuerProfile issuer,
        UpdateIssuerProfileRequest request
    )
    {
        if (
            existing.HasIssuedInvoice
            && (
                !string.Equals(issuer.LegalName, request.LegalName, StringComparison.Ordinal)
                || !string.Equals(
                    issuer.TaxId,
                    Raccord.SharedKernel.Parties.TaxId.Normalize(request.TaxId),
                    StringComparison.Ordinal
                )
            )
        )
        {
            throw new BusinessRuleViolationException(
                "issuer.identity-review-required",
                "Legal identity changes after issuing an invoice require a second administrator's review."
            );
        }
    }

    private static IssuerProfileResponse ToResponse(IssuerProfile issuer, bool hasIssuedInvoice)
    {
        var address = issuer.Address.ToAddress()!;
        return new IssuerProfileResponse(
            issuer.LegalName,
            issuer.TaxId,
            new AddressDto(
                address.Street,
                address.PostalCode,
                address.City,
                address.Province,
                address.CountryCode
            ),
            hasIssuedInvoice
        );
    }

    private static IssuerProfileSnapshot ToSnapshot(
        IssuerProfile issuer,
        int issuedInvoiceCount = 0
    ) =>
        new(
            issuer.Id,
            issuer.LegalName,
            issuer.TaxId,
            issuer.Address.ToAddress()!,
            issuedInvoiceCount
        );

    private IssuerIdentityChangeResponse ToResponse(IssuerIdentityChangeSnapshot request) =>
        new(
            request.Id,
            request.OldLegalName,
            request.OldTaxId,
            request.ProposedLegalName,
            request.ProposedTaxId,
            request.Reason,
            request.RequestedById,
            request.RequestedByName,
            request.RequestedAt,
            request.Status,
            request.DecidedById,
            request.DecidedByName,
            request.DecidedAt,
            string.Equals(request.RequestedById, actor.Id, StringComparison.Ordinal)
        );
}
