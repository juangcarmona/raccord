using Raccord.Contracts.Contacts;
using Raccord.Modules.Contacts.Contracts;
using Raccord.SharedKernel.Common;

namespace Raccord.Modules.Contacts;

/// <summary>Use cases for organizing Contacts with reusable tags and folders.</summary>
public sealed class ContactOrganizationDirectory(
    IContactOrganizationDirectory organization,
    IContactsUnitOfWork unitOfWork
)
{
    /// <summary>Lists reusable tags.</summary>
    public async Task<IReadOnlyList<ContactTagResponse>> ListTagsAsync(
        CancellationToken cancellationToken
    ) =>
        (await organization.ListTagsAsync(cancellationToken).ConfigureAwait(false))
            .Select(ToResponse)
            .ToArray();

    /// <summary>Creates a Contacts tag.</summary>
    public Task<ContactTagResponse> CreateTagAsync(
        CreateContactTagRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var name = request.Name?.Trim() ?? string.Empty;
        if (name.Length == 0 || name.Any(char.IsWhiteSpace))
        {
            throw new BusinessRuleViolationException(
                "contacts.tag-name-invalid",
                "A tag name is required and cannot contain whitespace."
            );
        }

        return unitOfWork.ExecuteAsync(
            async token =>
            {
                var tag = await organization.CreateTagAsync(name, token).ConfigureAwait(false);
                if (tag is null)
                {
                    throw new BusinessRuleViolationException(
                        "contacts.tag-duplicate",
                        "A tag with this name already exists."
                    );
                }

                return ToResponse(tag);
            },
            cancellationToken
        );
    }

    /// <summary>Deletes a tag only when it has no assignments.</summary>
    public Task<bool> DeleteTagAsync(Guid tagId, CancellationToken cancellationToken) =>
        unitOfWork.ExecuteAsync(
            async token =>
            {
                var existed = (await organization.ListTagsAsync(token).ConfigureAwait(false)).Any(
                    tag => tag.Id == tagId
                );
                if (!existed)
                {
                    return false;
                }

                if (!await organization.DeleteTagAsync(tagId, token).ConfigureAwait(false))
                {
                    throw new BusinessRuleViolationException(
                        "contacts.tag-in-use",
                        "Remove this tag from every contact before deleting it."
                    );
                }

                return true;
            },
            cancellationToken
        );

    /// <summary>Lists folders in stable display order.</summary>
    public async Task<IReadOnlyList<ContactFolderResponse>> ListFoldersAsync(
        CancellationToken cancellationToken
    ) =>
        (await organization.ListFoldersAsync(cancellationToken).ConfigureAwait(false))
            .Select(ToResponse)
            .ToArray();

    /// <summary>Creates a root or child folder.</summary>
    public Task<ContactFolderResponse?> CreateFolderAsync(
        CreateContactFolderRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var name = request.Name?.Trim() ?? string.Empty;
        if (name.Length == 0)
        {
            throw new BusinessRuleViolationException(
                "contacts.folder-name-required",
                "A folder name is required."
            );
        }

        return unitOfWork.ExecuteAsync(
            async token =>
            {
                var folder = await organization
                    .CreateFolderAsync(name, request.ParentId, token)
                    .ConfigureAwait(false);
                return folder is null ? null : ToResponse(folder);
            },
            cancellationToken
        );
    }

    /// <summary>Renames, reorders or moves a folder.</summary>
    public Task<ContactFolderResponse?> UpdateFolderAsync(
        Guid folderId,
        UpdateContactFolderRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var name = request.Name?.Trim() ?? string.Empty;
        if (name.Length == 0 || request.Position < 0)
        {
            throw new BusinessRuleViolationException(
                "contacts.folder-update-invalid",
                "A folder name is required and position cannot be negative."
            );
        }

        return unitOfWork.ExecuteAsync(
            async token =>
            {
                var folder = await organization
                    .UpdateFolderAsync(folderId, name, request.ParentId, request.Position, token)
                    .ConfigureAwait(false);
                if (folder is not null)
                {
                    return ToResponse(folder);
                }

                var exists = (await organization.ListFoldersAsync(token).ConfigureAwait(false)).Any(
                    item => item.Id == folderId
                );
                if (!exists)
                {
                    return null;
                }

                throw new BusinessRuleViolationException(
                    "contacts.folder-cycle",
                    "A folder cannot be moved into itself or one of its descendants."
                );
            },
            cancellationToken
        );
    }

    /// <summary>Deletes an empty folder.</summary>
    public Task<bool> DeleteFolderAsync(Guid folderId, CancellationToken cancellationToken) =>
        unitOfWork.ExecuteAsync(
            async token =>
            {
                var exists = (await organization.ListFoldersAsync(token).ConfigureAwait(false)).Any(
                    folder => folder.Id == folderId
                );
                if (!exists)
                {
                    return false;
                }

                if (!await organization.DeleteFolderAsync(folderId, token).ConfigureAwait(false))
                {
                    throw new BusinessRuleViolationException(
                        "contacts.folder-not-empty",
                        "Move its contacts and child folders before deleting this folder."
                    );
                }

                return true;
            },
            cancellationToken
        );

    /// <summary>Reads a contact's current tag assignments and folder.</summary>
    public async Task<ContactOrganizationResponse?> GetContactAsync(
        Guid contactId,
        CancellationToken cancellationToken
    )
    {
        var state = await organization
            .GetContactOrganizationAsync(contactId, cancellationToken)
            .ConfigureAwait(false);
        return state is null ? null : ToResponse(state);
    }

    /// <summary>Adds or removes one tag assignment.</summary>
    public Task<bool> SetTagAsync(
        Guid contactId,
        Guid tagId,
        bool assigned,
        CancellationToken cancellationToken
    ) =>
        unitOfWork.ExecuteAsync(
            token => organization.SetContactTagAsync(contactId, tagId, assigned, token),
            cancellationToken
        );

    /// <summary>Assigns or clears the contact's optional folder.</summary>
    public Task<bool> SetFolderAsync(
        Guid contactId,
        Guid? folderId,
        CancellationToken cancellationToken
    ) =>
        unitOfWork.ExecuteAsync(
            token => organization.SetContactFolderAsync(contactId, folderId, token),
            cancellationToken
        );

    private static ContactTagResponse ToResponse(ContactTagSnapshot tag) => new(tag.Id, tag.Name);

    private static ContactFolderResponse ToResponse(ContactFolderSnapshot folder) =>
        new(folder.Id, folder.Name, folder.ParentId, folder.Position);

    private static ContactOrganizationResponse ToResponse(ContactOrganizationSnapshot state) =>
        new(state.ContactId, state.FolderId, state.Tags.Select(ToResponse).ToArray());
}
