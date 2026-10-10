namespace Raccord.Domain.Parties;

/// <summary>A postal address as printed on invoices.</summary>
/// <param name="Street">Street, number and unit.</param>
/// <param name="PostalCode">Postal code.</param>
/// <param name="City">City or municipality.</param>
/// <param name="Province">Province, region or state; may be empty.</param>
/// <param name="CountryCode">ISO 3166-1 alpha-2 country code.</param>
public sealed record PostalAddress(
    string Street,
    string PostalCode,
    string City,
    string Province,
    string CountryCode
)
{
    /// <summary>Whether the fields required on an invoice are present.</summary>
    public bool IsComplete =>
        !string.IsNullOrWhiteSpace(Street)
        && !string.IsNullOrWhiteSpace(PostalCode)
        && !string.IsNullOrWhiteSpace(City)
        && CountryCode is { Length: 2 };
}
