namespace Raccord.Modules.Contacts.Contracts;

/// <summary>Reads and stages changes to shared company and private-person contacts.</summary>
public interface IContactDirectory
{
    /// <summary>Finds a contact by its stable identifier.</summary>
    Task<ContactSnapshot?> FindContactAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Checks whether a contact number is already assigned.</summary>
    Task<bool> ContactNumberExistsAsync(string contactNumber, CancellationToken cancellationToken);

    /// <summary>Returns a page ordered by contact number and identifier.</summary>
    Task<ContactPage> ListContactsAsync(
        int page,
        int pageSize,
        ContactRole? role,
        CancellationToken cancellationToken
    );

    /// <summary>Adds a contact to the current unit of work.</summary>
    Task AddContactAsync(ContactSnapshot contact, CancellationToken cancellationToken);

    /// <summary>Updates an existing contact without changing its identifier.</summary>
    Task UpdateContactAsync(ContactSnapshot contact, CancellationToken cancellationToken);
}
