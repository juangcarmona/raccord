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
                )
            );
    }

    /// <summary>Defines or updates the issuer profile used by future invoices.</summary>
    public async Task<IssuerProfileResponse> UpdateIssuerAsync(
        UpdateIssuerProfileRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var address = request.Address is null
            ? null
            : new PostalAddress(
                request.Address.Street,
                request.Address.PostalCode,
                request.Address.City,
                request.Address.Province,
                request.Address.CountryCode
            );
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
            issuer.Update(request.LegalName ?? string.Empty, request.TaxId, address);
            workspace.UpdateIssuer(ToSnapshot(issuer));
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
        return new IssuerProfileResponse(
            issuer.LegalName,
            issuer.TaxId,
            new AddressDto(
                issuer.Address.ToAddress()!.Street,
                issuer.Address.ToAddress()!.PostalCode,
                issuer.Address.ToAddress()!.City,
                issuer.Address.ToAddress()!.Province,
                issuer.Address.ToAddress()!.CountryCode
            )
        );
    }

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

    private static IssuerProfileSnapshot ToSnapshot(IssuerProfile issuer) =>
        new(issuer.Id, issuer.LegalName, issuer.TaxId, issuer.Address.ToAddress()!);
}
