namespace Raccord.Contracts.Contacts;

/// <summary>Creates a reusable tag for Contacts.</summary>
public sealed record CreateContactTagRequest(string Name);

/// <summary>A tag available to Contacts in this deployment.</summary>
public sealed record ContactTagResponse(Guid Id, string Name);

/// <summary>Creates a root folder or a child folder.</summary>
public sealed record CreateContactFolderRequest(string Name, Guid? ParentId);

/// <summary>Changes a folder's name, parent or sibling position.</summary>
public sealed record UpdateContactFolderRequest(string Name, Guid? ParentId, int Position);

/// <summary>A node in the Contacts folder hierarchy.</summary>
public sealed record ContactFolderResponse(Guid Id, string Name, Guid? ParentId, int Position);

/// <summary>Sets or clears the contact's optional folder.</summary>
public sealed record AssignContactFolderRequest(Guid? FolderId);

/// <summary>Current organization data assigned to one contact.</summary>
public sealed record ContactOrganizationResponse(
    Guid ContactId,
    Guid? FolderId,
    IReadOnlyList<ContactTagResponse> Tags
);
