using System.Net.Mail;
using Raccord.SharedKernel.Common;

namespace Raccord.Modules.Contacts.Domain;

/// <summary>A person associated with one Contacts record.</summary>
internal sealed class ContactPerson
{
    private ContactPerson()
    {
        Name = string.Empty;
    }

    private ContactPerson(Guid contactId, string name, string? email, string? phone)
    {
        Id = Guid.CreateVersion7();
        ContactId = contactId;
        Name = name;
        Email = email;
        Phone = phone;
    }

    private ContactPerson(
        Guid id,
        Guid contactId,
        string name,
        string? email,
        string? phone,
        bool isDefault,
        bool isAdministrative
    )
        : this(contactId, name, email, phone)
    {
        Id = id;
        IsDefault = isDefault;
        IsAdministrative = isAdministrative;
    }

    internal Guid Id { get; private set; }

    internal Guid ContactId { get; private set; }

    internal string Name { get; private set; }

    internal string? Email { get; private set; }

    internal string? Phone { get; private set; }

    internal bool IsDefault { get; private set; }

    internal bool IsAdministrative { get; private set; }

    internal static ContactPerson Create(Guid contactId, string name, string? email, string? phone)
    {
        if (contactId == Guid.Empty)
        {
            throw new BusinessRuleViolationException(
                "contacts.contact-person-contact-required",
                "A contact person must belong to a contact."
            );
        }

        var details = ValidateDetails(name, email, phone);

        return new ContactPerson(contactId, details.Name, details.Email, details.Phone);
    }

    internal static ContactPerson Restore(
        Guid id,
        Guid contactId,
        string name,
        string? email,
        string? phone,
        bool isDefault,
        bool isAdministrative
    )
    {
        var details = ValidateDetails(name, email, phone);
        return new ContactPerson(
            id,
            contactId,
            details.Name,
            details.Email,
            details.Phone,
            isDefault,
            isAdministrative
        );
    }

    internal void Update(Guid contactId, string name, string? email, string? phone)
    {
        if (contactId != ContactId)
        {
            throw new BusinessRuleViolationException(
                "contacts.contact-person-parent-immutable",
                "A contact person cannot be moved to another contact."
            );
        }

        var details = ValidateDetails(name, email, phone);

        Name = details.Name;
        Email = details.Email;
        Phone = details.Phone;
    }

    internal static void AssignDesignations(
        ContactPerson person,
        IReadOnlyCollection<ContactPerson> people,
        bool isDefault,
        bool isAdministrative
    )
    {
        ArgumentNullException.ThrowIfNull(person);
        ArgumentNullException.ThrowIfNull(people);

        if (
            people.Count == 0
            || people.Any(candidate => candidate.ContactId != person.ContactId)
            || people.All(candidate => candidate.Id != person.Id)
        )
        {
            throw new BusinessRuleViolationException(
                "contacts.contact-person-designation-scope-invalid",
                "Designations can only be changed within the contact's people."
            );
        }

        if (
            isAdministrative
            && people.Any(candidate => candidate.Id != person.Id && candidate.IsAdministrative)
        )
        {
            throw new BusinessRuleViolationException(
                "contacts.contact-person-administrative-already-assigned",
                "A contact can have at most one administrative contact person."
            );
        }

        if (isDefault)
        {
            foreach (var candidate in people.Where(candidate => candidate.Id != person.Id))
            {
                candidate.IsDefault = false;
            }
        }

        person.IsDefault = isDefault;
        person.IsAdministrative = isAdministrative;
    }

    private static (string Name, string? Email, string? Phone) ValidateDetails(
        string name,
        string? email,
        string? phone
    )
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleViolationException(
                "contacts.contact-person-name-required",
                "Contact person name is required."
            );
        }

        var normalizedEmail = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
        if (normalizedEmail is not null && !IsValidEmail(normalizedEmail))
        {
            throw new BusinessRuleViolationException(
                "contacts.contact-person-email-invalid",
                "Contact person email is not valid."
            );
        }

        return (
            name.Trim(),
            normalizedEmail,
            string.IsNullOrWhiteSpace(phone) ? null : phone.Trim()
        );
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var parsed = new MailAddress(email);
            return parsed.Address.Equals(email, StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
