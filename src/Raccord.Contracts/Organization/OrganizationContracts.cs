using Raccord.Contracts.Common;

namespace Raccord.Contracts.Organization;

/// <summary>Sets the legal issuer details used on future invoices.</summary>
/// <param name="LegalName">Registered legal name.</param>
/// <param name="TaxId">Tax ID.</param>
/// <param name="Address">Registered address.</param>
public sealed record UpdateIssuerProfileRequest(string LegalName, string TaxId, AddressDto Address);

/// <summary>Requests a reviewed change to the issuer legal identity.</summary>
public sealed record CreateIssuerIdentityChangeRequest(
    string LegalName,
    string TaxId,
    string Reason
);

/// <summary>Decides a pending issuer legal-identity change.</summary>
public sealed record DecideIssuerIdentityChangeRequest(bool Approve);

/// <summary>Issuer identity change request and its decision history.</summary>
public sealed record IssuerIdentityChangeResponse(
    Guid Id,
    string OldLegalName,
    string OldTaxId,
    string ProposedLegalName,
    string ProposedTaxId,
    string Reason,
    string RequestedById,
    string RequestedByName,
    DateTimeOffset RequestedAt,
    string Status,
    string? DecidedById,
    string? DecidedByName,
    DateTimeOffset? DecidedAt,
    bool IsRequester = false
);

/// <summary>The legal issuer of invoices.</summary>
/// <param name="LegalName">Registered legal name.</param>
/// <param name="TaxId">Normalized tax ID.</param>
/// <param name="Address">Registered address.</param>
/// <param name="HasIssuedInvoice">Whether at least one invoice has been issued for this profile.</param>
public sealed record IssuerProfileResponse(
    string LegalName,
    string TaxId,
    AddressDto Address,
    bool HasIssuedInvoice = false
);
