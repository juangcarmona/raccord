namespace Raccord.Modules.Contacts.Contracts;

/// <summary>A tag as persisted by Contacts.</summary>
public sealed record ContactTagSnapshot(Guid Id, string Name);
