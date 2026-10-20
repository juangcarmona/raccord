using Raccord.Contracts.Common;
using Raccord.Contracts.Contacts;
using Raccord.Modules.Contacts.Contracts;
using Raccord.Modules.Contacts.Domain;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Parties;

namespace Raccord.Modules.Contacts;

/// <summary>Use cases for maintaining and browsing the shared contacts directory.</summary>
public sealed class ContactsDirectory(IContactDirectory contacts, IContactsUnitOfWork unitOfWork)
{
    private const int MaximumPageSize = 100;

    /// <summary>Creates a company or private-person contact.</summary>
    public async Task<ContactResponse> CreateAsync(
        SaveContactRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var contact = Contact.Register(
            ParseType(request.Type),
            request.LegalName,
            request.TaxId,
            ToAddress(request.Address),
            request.Email,
            request.ContactNumber,
            ParseRoles(request.Roles)
        );
        var snapshot = ToSnapshot(contact.Id, contact);
        await contacts.AddContactAsync(snapshot, cancellationToken).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        var saved = await contacts
            .FindContactAsync(contact.Id, cancellationToken)
            .ConfigureAwait(false);
        return ToResponse(saved!)!;
    }

    /// <summary>Reads one contact by its stable identifier.</summary>
    public async Task<ContactResponse?> GetAsync(Guid id, CancellationToken cancellationToken) =>
        ToResponse(await contacts.FindContactAsync(id, cancellationToken).ConfigureAwait(false));

    /// <summary>Updates the editable details and roles of an existing contact.</summary>
    public async Task<ContactResponse?> UpdateAsync(
        Guid id,
        SaveContactRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var existing = await contacts.FindContactAsync(id, cancellationToken).ConfigureAwait(false);
        if (existing is null)
        {
            return null;
        }

        var contact = Contact.Register(
            ParseType(request.Type),
            request.LegalName,
            request.TaxId,
            ToAddress(request.Address),
            request.Email,
            request.ContactNumber,
            ParseRoles(request.Roles)
        );
        var updated = ToSnapshot(id, contact);
        await contacts.UpdateContactAsync(updated, cancellationToken).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(
            await contacts.FindContactAsync(id, cancellationToken).ConfigureAwait(false)
        );
    }

    /// <summary>Lists contacts in deterministic pages, optionally filtered by role.</summary>
    public async Task<ContactPageResponse> ListAsync(
        int page,
        int pageSize,
        string? role,
        CancellationToken cancellationToken
    )
    {
        if (page < 1 || page > 1_000_000)
        {
            throw new BusinessRuleViolationException(
                "contacts.page-invalid",
                "Page must be between 1 and 1000000."
            );
        }

        if (pageSize < 1 || pageSize > MaximumPageSize)
        {
            throw new BusinessRuleViolationException(
                "contacts.page-size-invalid",
                $"Page size must be between 1 and {MaximumPageSize}."
            );
        }

        ContactRole? roleFilter = string.IsNullOrWhiteSpace(role) ? null : ParseRole(role);
        var result = await contacts
            .ListContactsAsync(page, pageSize, roleFilter, cancellationToken)
            .ConfigureAwait(false);
        return new ContactPageResponse(
            result.Items.Select(item => ToResponse(item)!).ToArray(),
            result.Page,
            result.PageSize,
            result.TotalCount,
            result.HasNextPage,
            result.HasPreviousPage
        );
    }

    private static ContactType ParseType(string? value) =>
        Enum.TryParse<ContactType>(value, ignoreCase: true, out var type) && Enum.IsDefined(type)
            ? type
            : throw new BusinessRuleViolationException(
                "contacts.type-unsupported",
                "Contact type is not supported."
            );

    private static ContactRole[] ParseRoles(IReadOnlyList<string>? roles)
    {
        if (roles is null)
        {
            throw new BusinessRuleViolationException(
                "contacts.role-required",
                "At least one contact role is required."
            );
        }

        return roles.Select(ParseRole).Distinct().ToArray();
    }

    private static ContactRole ParseRole(string value) =>
        Enum.TryParse<ContactRole>(value, ignoreCase: true, out var role) && Enum.IsDefined(role)
            ? role
            : throw new BusinessRuleViolationException(
                "contacts.role-unsupported",
                "Contact role is not supported."
            );

    private static PostalAddress? ToAddress(AddressDto? address) =>
        address is null
            ? null
            : new PostalAddress(
                address.Street,
                address.PostalCode,
                address.City,
                address.Province,
                address.CountryCode
            );

    private static ContactSnapshot ToSnapshot(Guid id, Contact contact) =>
        new(
            id,
            contact.Type,
            contact.ContactNumber,
            contact.LegalName,
            contact.TaxId,
            contact.BillingAddress.ToAddress(),
            contact.Email,
            contact.Roles
        );

    private static ContactResponse? ToResponse(ContactSnapshot? contact) =>
        contact is null
            ? null
            : new ContactResponse(
                contact.Id,
                contact.Type.ToString(),
                contact.ContactNumber ?? string.Empty,
                contact.LegalName,
                contact.TaxId,
                contact.Address is null
                    ? null
                    : new AddressDto(
                        contact.Address.Street,
                        contact.Address.PostalCode,
                        contact.Address.City,
                        contact.Address.Province,
                        contact.Address.CountryCode
                    ),
                contact.Email,
                contact
                    .Roles.Select(item => item.ToString())
                    .Order(StringComparer.Ordinal)
                    .ToArray(),
                contact.IsInvoiceable
            );
}
