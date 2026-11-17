namespace Raccord.Modules.Contacts.Contracts;

/// <summary>Contact organization state returned by the Contacts store.</summary>
public sealed record ContactOrganizationSnapshot(
    Guid ContactId,
    Guid? FolderId,
    IReadOnlyList<ContactTagSnapshot> Tags
);
