namespace Raccord.Domain.Parties;

/// <summary>Flat, persistence-friendly storage of an optional postal address.</summary>
public sealed class AddressFields
{
    /// <summary>Street, number and unit.</summary>
    public string? Street { get; private set; }

    /// <summary>Postal code.</summary>
    public string? PostalCode { get; private set; }

    /// <summary>City or municipality.</summary>
    public string? City { get; private set; }

    /// <summary>Province, region or state.</summary>
    public string? Province { get; private set; }

    /// <summary>ISO 3166-1 alpha-2 country code.</summary>
    public string? CountryCode { get; private set; }

    /// <summary>The stored address, or null when no address was given.</summary>
    public PostalAddress? ToAddress() =>
        Street is null ? null : new PostalAddress(Street, PostalCode ?? string.Empty, City ?? string.Empty, Province ?? string.Empty, CountryCode ?? string.Empty);

    /// <summary>Replaces the stored address.</summary>
    public void Set(PostalAddress? address)
    {
        Street = address?.Street.Trim();
        PostalCode = address?.PostalCode.Trim();
        City = address?.City.Trim();
        Province = address?.Province.Trim();
        CountryCode = address?.CountryCode.Trim().ToUpperInvariant();
    }
}
