using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Parties;

namespace Raccord.Modules.Workspace.Domain;

/// <summary>The legal identity of the organization issuing invoices.</summary>
internal sealed class IssuerProfile
{
    private IssuerProfile()
    {
        LegalName = string.Empty;
        TaxId = string.Empty;
        Address = new AddressFields();
    }

    private IssuerProfile(string legalName, string taxId, PostalAddress address)
    {
        Id = Guid.CreateVersion7();
        LegalName = legalName;
        TaxId = taxId;
        Address = new AddressFields();
        Address.Set(address);
    }

    private IssuerProfile(Guid id, string legalName, string taxId, PostalAddress address)
        : this(legalName, taxId, address)
    {
        Id = id;
    }

    internal Guid Id { get; private set; }

    internal string LegalName { get; private set; }

    internal string TaxId { get; private set; }

    internal AddressFields Address { get; private set; }

    internal static IssuerProfile Define(string legalName, string? taxId, PostalAddress address)
    {
        var profile = new IssuerProfile { Id = Guid.CreateVersion7() };
        profile.Update(legalName, taxId, address);
        return profile;
    }

    internal static IssuerProfile Load(
        Guid id,
        string legalName,
        string taxId,
        PostalAddress address
    ) => new(id, legalName, taxId, address);

    internal void Update(string legalName, string? taxId, PostalAddress address)
    {
        if (string.IsNullOrWhiteSpace(legalName))
        {
            throw new BusinessRuleViolationException(
                "issuer.legal-name-missing",
                "Issuer legal name is required."
            );
        }

        if (string.IsNullOrWhiteSpace(taxId))
        {
            throw new BusinessRuleViolationException(
                "issuer.tax-id-missing",
                "Issuer tax ID is required."
            );
        }

        if (!address.IsComplete)
        {
            throw new BusinessRuleViolationException(
                "issuer.address-missing",
                "Issuer address must contain the required fields."
            );
        }

        LegalName = legalName.Trim();
        TaxId = Raccord.SharedKernel.Parties.TaxId.Normalize(taxId) ?? string.Empty;
        Address.Set(address);
    }

    internal LegalParty ToLegalParty() => new(LegalName, TaxId, Address.ToAddress());
}
