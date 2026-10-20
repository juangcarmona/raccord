namespace Raccord.Modules.Contacts.Contracts;

/// <summary>Stores Contacts tags, assignments, folders and directory filters.</summary>
public interface IContactOrganizationDirectory
{
    /// <summary>Lists tags in the current deployment.</summary>
    Task<IReadOnlyList<ContactTagSnapshot>> ListTagsAsync(CancellationToken cancellationToken);

    /// <summary>Creates a new tag, returning null when its name already exists.</summary>
    Task<ContactTagSnapshot?> CreateTagAsync(string name, CancellationToken cancellationToken);

    /// <summary>Deletes an unassigned tag, returning false when it is missing or in use.</summary>
    Task<bool> DeleteTagAsync(Guid tagId, CancellationToken cancellationToken);

    /// <summary>Lists folders in display order.</summary>
    Task<IReadOnlyList<ContactFolderSnapshot>> ListFoldersAsync(
        CancellationToken cancellationToken
    );

    /// <summary>Creates a folder if the parent exists.</summary>
    Task<ContactFolderSnapshot?> CreateFolderAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken
    );

    /// <summary>Updates a folder if the new parent does not create a cycle.</summary>
    Task<ContactFolderSnapshot?> UpdateFolderAsync(
        Guid folderId,
        string name,
        Guid? parentId,
        int position,
        CancellationToken cancellationToken
    );

    /// <summary>Deletes a folder only when it contains no contacts or child folders.</summary>
    Task<bool> DeleteFolderAsync(Guid folderId, CancellationToken cancellationToken);

    /// <summary>Reads a contact's assigned folder and tags, or returns null for an unknown contact.</summary>
    Task<ContactOrganizationSnapshot?> GetContactOrganizationAsync(
        Guid contactId,
        CancellationToken cancellationToken
    );

    /// <summary>Assigns or removes a tag for a contact; false means either id is unknown.</summary>
    Task<bool> SetContactTagAsync(
        Guid contactId,
        Guid tagId,
        bool assigned,
        CancellationToken cancellationToken
    );

    /// <summary>Assigns or clears a contact's folder; false means an id is unknown.</summary>
    Task<bool> SetContactFolderAsync(
        Guid contactId,
        Guid? folderId,
        CancellationToken cancellationToken
    );
}
