namespace Raccord.Modules.Contacts.Contracts;

/// <summary>Contacts-owned details and designations for a person affiliated with a contact.</summary>
public sealed record ContactPersonSnapshot(
    Guid Id,
    Guid ContactId,
    string Name,
    string? Email,
    string? Phone,
    bool IsDefault,
    bool IsAdministrative
);
