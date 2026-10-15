#pragma warning disable CS1591

namespace Raccord.Contracts.Integrations;

public sealed record CompanyRegistryMatchResponse(
    string RegistrationNumber,
    string LegalName,
    string? Street,
    string? PostalCode,
    string? City,
    string? CountryCode
);

#pragma warning restore CS1591
