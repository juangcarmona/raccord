using Raccord.Contracts.Contacts;
using Raccord.Modules.Contacts.Contracts;
using Raccord.Modules.Contacts.Domain;

namespace Raccord.Modules.Contacts;

/// <summary>Use cases for maintaining people affiliated with a shared contact.</summary>
public sealed class ContactPeopleDirectory(
    IContactDirectory contacts,
    IContactPeopleDirectory people,
    IContactsUnitOfWork unitOfWork
)
{
    /// <summary>Lists a contact's people, or returns null when the parent contact is missing.</summary>
    public async Task<IReadOnlyList<ContactPersonResponse>?> ListAsync(
        Guid contactId,
        CancellationToken cancellationToken
    )
    {
        if (
            await contacts.FindContactAsync(contactId, cancellationToken).ConfigureAwait(false)
            is null
        )
        {
            return null;
        }

        return (await people.ListAsync(contactId, cancellationToken).ConfigureAwait(false))
            .Select(ToResponse)
            .ToArray();
    }

    /// <summary>Reads a person only when it belongs to the contact in the route.</summary>
    public async Task<ContactPersonResponse?> GetAsync(
        Guid contactId,
        Guid personId,
        CancellationToken cancellationToken
    )
    {
        var person = await people
            .FindAsync(contactId, personId, cancellationToken)
            .ConfigureAwait(false);
        return person is null ? null : ToResponse(person);
    }

    /// <summary>Creates a person under a contact and atomically applies its designations.</summary>
    public Task<ContactPersonResponse?> CreateAsync(
        Guid contactId,
        SaveContactPersonRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        return unitOfWork.ExecuteAsync(
            async token =>
            {
                if (!await people.LockContactAsync(contactId, token).ConfigureAwait(false))
                {
                    return null;
                }

                var existingSnapshots = await people
                    .ListAsync(contactId, token)
                    .ConfigureAwait(false);
                var existing = existingSnapshots.Select(ToDomain).ToList();
                var created = ContactPerson.Create(
                    contactId,
                    request.Name,
                    request.Email,
                    request.Phone
                );
                var domainPeople = existing.Append(created).ToArray();
                ContactPerson.AssignDesignations(
                    created,
                    domainPeople,
                    request.IsDefault,
                    request.IsAdministrative
                );

                foreach (var person in existing)
                {
                    var previous = existingSnapshots.Single(item => item.Id == person.Id);
                    var updated = ToSnapshot(person);
                    if (previous != updated)
                    {
                        await people.UpdateAsync(updated, token).ConfigureAwait(false);
                    }
                }

                await people.AddAsync(ToSnapshot(created), token).ConfigureAwait(false);
                return ToResponse(ToSnapshot(created));
            },
            cancellationToken
        );
    }

    /// <summary>Edits a person's details and designations without changing its parent.</summary>
    public Task<ContactPersonResponse?> UpdateAsync(
        Guid contactId,
        Guid personId,
        SaveContactPersonRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        return unitOfWork.ExecuteAsync(
            async token =>
            {
                if (!await people.LockContactAsync(contactId, token).ConfigureAwait(false))
                {
                    return null;
                }

                var existingSnapshots = await people
                    .ListAsync(contactId, token)
                    .ConfigureAwait(false);
                var domainPeople = existingSnapshots.Select(ToDomain).ToArray();
                var person = domainPeople.SingleOrDefault(item => item.Id == personId);
                if (person is null)
                {
                    return null;
                }

                person.Update(contactId, request.Name, request.Email, request.Phone);
                ContactPerson.AssignDesignations(
                    person,
                    domainPeople,
                    request.IsDefault,
                    request.IsAdministrative
                );

                foreach (var updatedPerson in domainPeople)
                {
                    var previous = existingSnapshots.Single(item => item.Id == updatedPerson.Id);
                    var updated = ToSnapshot(updatedPerson);
                    if (previous != updated)
                    {
                        await people.UpdateAsync(updated, token).ConfigureAwait(false);
                    }
                }

                return ToResponse(ToSnapshot(person));
            },
            cancellationToken
        );
    }

    private static ContactPerson ToDomain(ContactPersonSnapshot person) =>
        ContactPerson.Restore(
            person.Id,
            person.ContactId,
            person.Name,
            person.Email,
            person.Phone,
            person.IsDefault,
            person.IsAdministrative
        );

    private static ContactPersonSnapshot ToSnapshot(ContactPerson person) =>
        new(
            person.Id,
            person.ContactId,
            person.Name,
            person.Email,
            person.Phone,
            person.IsDefault,
            person.IsAdministrative
        );

    private static ContactPersonResponse ToResponse(ContactPersonSnapshot person) =>
        new(
            person.Id,
            person.ContactId,
            person.Name,
            person.Email,
            person.Phone,
            person.IsDefault,
            person.IsAdministrative
        );
}
