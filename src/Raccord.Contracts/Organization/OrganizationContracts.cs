using Raccord.Contracts.Common;

namespace Raccord.Contracts.Organization;

/// <summary>Sets the legal issuer details used on future invoices.</summary>
/// <param name="LegalName">Registered legal name.</param>
/// <param name="TaxId">Tax ID.</param>
/// <param name="Address">Registered address.</param>
public sealed record UpdateIssuerProfileRequest(string LegalName, string TaxId, AddressDto Address);

/// <summary>The legal issuer of invoices.</summary>
/// <param name="LegalName">Registered legal name.</param>
/// <param name="TaxId">Normalized tax ID.</param>
/// <param name="Address">Registered address.</param>
public sealed record IssuerProfileResponse(string LegalName, string TaxId, AddressDto Address);
