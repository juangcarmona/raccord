namespace Raccord.Contracts.Common;

/// <summary>A postal address.</summary>
/// <param name="Street">Street, number and unit.</param>
/// <param name="PostalCode">Postal code.</param>
/// <param name="City">City or municipality.</param>
/// <param name="Province">Province, region or state.</param>
/// <param name="CountryCode">ISO 3166-1 alpha-2 country code.</param>
public sealed record AddressDto(
    string Street,
    string PostalCode,
    string City,
    string Province,
    string CountryCode
);

/// <summary>A legal party as printed on an invoice.</summary>
/// <param name="LegalName">Registered legal name.</param>
/// <param name="TaxId">Tax identification number.</param>
/// <param name="Address">Address, if known.</param>
public sealed record PartyResponse(string LegalName, string? TaxId, AddressDto? Address);

/// <summary>A reason an operation cannot proceed, identified by a stable code the UI can localize.</summary>
/// <param name="Code">Stable rule code.</param>
/// <param name="Message">English explanation.</param>
public sealed record ValidationIssueResponse(string Code, string Message);
