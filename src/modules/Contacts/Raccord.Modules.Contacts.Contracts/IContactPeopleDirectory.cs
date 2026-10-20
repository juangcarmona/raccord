namespace Raccord.Modules.Contacts.Contracts;

/// <summary>Reads and stages contact-person records through the Contacts persistence adapter.</summary>
public interface IContactPeopleDirectory
{
    /// <summary>Locks a contact while its people and designations are changed.</summary>
    Task<bool> LockContactAsync(Guid contactId, CancellationToken cancellationToken);

    /// <summary>Lists all people belonging to the given contact.</summary>
    Task<IReadOnlyList<ContactPersonSnapshot>> ListAsync(
        Guid contactId,
        CancellationToken cancellationToken
    );

    /// <summary>Finds one person only when it belongs to the given contact.</summary>
    Task<ContactPersonSnapshot?> FindAsync(
        Guid contactId,
        Guid personId,
        CancellationToken cancellationToken
    );

    /// <summary>Adds a contact person to the current unit of work.</summary>
    Task AddAsync(ContactPersonSnapshot person, CancellationToken cancellationToken);

    /// <summary>Updates a contact person under its existing parent.</summary>
    Task UpdateAsync(ContactPersonSnapshot person, CancellationToken cancellationToken);
}
