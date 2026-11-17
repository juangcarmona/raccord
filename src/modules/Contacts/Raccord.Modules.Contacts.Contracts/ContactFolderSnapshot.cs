namespace Raccord.Modules.Contacts.Contracts;

/// <summary>A folder as persisted by Contacts.</summary>
public sealed record ContactFolderSnapshot(Guid Id, string Name, Guid? ParentId, int Position);
