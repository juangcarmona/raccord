using Raccord.Modules.Contacts.Contracts;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Parties;

namespace Raccord.Modules.Contacts.Domain;

/// <summary>A company or private person used by projects and invoicing.</summary>
internal sealed class Contact
{
    private Contact()
    {
        LegalName = string.Empty;
    }

    private Contact(
        ContactType type,
        string legalName,
        string? taxId,
        PostalAddress? address,
        string? email,
        string? contactNumber,
        IReadOnlyCollection<ContactRole> roles
    )
    {
        Id = Guid.CreateVersion7();
        Type = type;
        LegalName = legalName;
        TaxId = Raccord.SharedKernel.Parties.TaxId.Normalize(taxId);
        BillingAddress.Set(address);
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
        ContactNumber = string.IsNullOrWhiteSpace(contactNumber) ? null : contactNumber.Trim();
        Roles = roles.ToHashSet();
    }

    internal Guid Id { get; private set; }

    internal ContactType Type { get; private set; }

    internal string LegalName { get; private set; }

    internal string? TaxId { get; private set; }

    internal AddressFields BillingAddress { get; private set; } = new();

    internal string? Email { get; private set; }

    internal string? ContactNumber { get; private set; }

    internal IReadOnlySet<ContactRole> Roles { get; private set; } = new HashSet<ContactRole>();

    internal static Contact Register(
        ContactType type,
        string legalName,
        string? taxId,
        PostalAddress? address,
        string? email,
        string? contactNumber,
        IReadOnlyCollection<ContactRole> roles
    )
    {
        ArgumentNullException.ThrowIfNull(roles);

        if (!Enum.IsDefined(type))
        {
            throw new BusinessRuleViolationException(
                "contacts.type-unsupported",
                "Contact type is not supported."
            );
        }

        if (string.IsNullOrWhiteSpace(legalName))
        {
            throw new BusinessRuleViolationException(
                "contacts.legal-name-required",
                "Contact legal name is required."
            );
        }

        if (roles.Count == 0)
        {
            throw new BusinessRuleViolationException(
                "contacts.role-required",
                "At least one contact role is required."
            );
        }

        if (roles.Any(role => !Enum.IsDefined(role)))
        {
            throw new BusinessRuleViolationException(
                "contacts.role-unsupported",
                "Contact role is not supported."
            );
        }

        return new Contact(type, legalName.Trim(), taxId, address, email, contactNumber, roles);
    }

    internal void Update(
        ContactType type,
        string legalName,
        string? taxId,
        PostalAddress? address,
        string? email,
        string? contactNumber,
        IReadOnlyCollection<ContactRole> roles
    )
    {
        var updated = Register(type, legalName, taxId, address, email, contactNumber, roles);
        Type = updated.Type;
        LegalName = updated.LegalName;
        TaxId = updated.TaxId;
        BillingAddress.Set(updated.BillingAddress.ToAddress());
        Email = updated.Email;
        ContactNumber = updated.ContactNumber;
        Roles = updated.Roles;
    }

    internal LegalParty ToLegalParty() => new(LegalName, TaxId, BillingAddress.ToAddress());
}
