using Raccord.Domain.Common;
using Raccord.Domain.Parties;

namespace Raccord.Domain.Organization;

/// <summary>The legal entity that issues invoices from this Raccord installation.</summary>
public sealed class IssuerProfile
{
    private IssuerProfile()
    {
        LegalName = string.Empty;
        TaxId = string.Empty;
    }

    /// <summary>Profile identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Registered legal name.</summary>
    public string LegalName { get; private set; }

    /// <summary>Normalized tax ID.</summary>
    public string TaxId { get; private set; }

    /// <summary>Registered address.</summary>
    public AddressFields Address { get; private set; } = new();

    /// <summary>Defines the issuer. All fields required on an invoice must be present.</summary>
    public static IssuerProfile Define(string legalName, string? taxId, PostalAddress address)
    {
        var profile = new IssuerProfile { Id = Guid.CreateVersion7() };
        profile.Update(legalName, taxId, address);
        return profile;
    }

    /// <summary>Changes the issuer details used by future invoices. Issued invoices keep their snapshot.</summary>
    public void Update(string legalName, string? taxId, PostalAddress address)
    {
        if (string.IsNullOrWhiteSpace(legalName))
        {
            throw new BusinessRuleViolationException(
                "issuer.legal-name-missing",
                "The issuer legal name is required."
            );
        }

        var normalizedTaxId =
            Parties.TaxId.Normalize(taxId)
            ?? throw new BusinessRuleViolationException(
                "issuer.tax-id-missing",
                "The issuer tax ID is required."
            );

        if (address is not { IsComplete: true })
        {
            throw new BusinessRuleViolationException(
                "issuer.address-missing",
                "The issuer address is incomplete."
            );
        }

        LegalName = legalName.Trim();
        TaxId = normalizedTaxId;
        Address.Set(address);
    }

    /// <summary>The issuer's legal identity for an invoice.</summary>
    public LegalParty ToLegalParty() => new(LegalName, TaxId, Address.ToAddress());
}
