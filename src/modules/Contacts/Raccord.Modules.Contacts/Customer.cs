using Raccord.Contracts.Contacts;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Parties;

namespace Raccord.Modules.Contacts.Domain;

/// <summary>A company or person Raccord bills.</summary>
internal sealed class Customer
{
    private Customer()
    {
        LegalName = string.Empty;
    }

    private Customer(
        string legalName,
        ContactType type,
        string? taxId,
        PostalAddress? address,
        string? email
    )
    {
        Id = Guid.CreateVersion7();
        LegalName = legalName;
        Type = type;
        TaxId = taxId;
        BillingAddress.Set(address);
        Email = email;
    }

    /// <summary>Customer identifier.</summary>
    internal Guid Id { get; private set; }

    /// <summary>Registered legal name.</summary>
    internal string LegalName { get; private set; }

    internal ContactType Type { get; private set; }

    /// <summary>Normalized tax ID, if known.</summary>
    internal string? TaxId { get; private set; }

    /// <summary>Billing address.</summary>
    internal AddressFields BillingAddress { get; private set; } = new();

    /// <summary>Billing e-mail address.</summary>
    internal string? Email { get; private set; }

    /// <summary>Registers a customer. Tax ID and address may be completed before invoicing.</summary>
    internal static Customer Register(
        string legalName,
        ContactType type,
        string? taxId,
        PostalAddress? address,
        string? email
    )
    {
        if (string.IsNullOrWhiteSpace(legalName))
        {
            throw new BusinessRuleViolationException(
                "customers.legal-name-required",
                "Customer legal name is required."
            );
        }

        return new Customer(
            legalName.Trim(),
            type,
            Raccord.SharedKernel.Parties.TaxId.Normalize(taxId),
            address,
            string.IsNullOrWhiteSpace(email) ? null : email.Trim()
        );
    }

    internal static Customer Register(
        string legalName,
        string? taxId,
        PostalAddress? address,
        string? email
    ) => Register(legalName, ContactType.Customer, taxId, address, email);

    internal void Update(
        string legalName,
        ContactType type,
        string? taxId,
        PostalAddress? address,
        string? email
    )
    {
        if (string.IsNullOrWhiteSpace(legalName))
        {
            throw new BusinessRuleViolationException(
                "contacts.legal-name-required",
                "Contact legal name is required."
            );
        }

        LegalName = legalName.Trim();
        Type = type;
        TaxId = Raccord.SharedKernel.Parties.TaxId.Normalize(taxId);
        BillingAddress.Set(address);
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
    }

    /// <summary>The customer's legal identity for an invoice.</summary>
    internal LegalParty ToLegalParty() => new(LegalName, TaxId, BillingAddress.ToAddress());
}
