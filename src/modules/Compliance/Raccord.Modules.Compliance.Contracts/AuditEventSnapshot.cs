namespace Raccord.Modules.Compliance.Contracts;

/// <summary>Read model returned by the compliance audit trail.</summary>
public sealed record AuditEventSnapshot(
    Guid Id,
    DateTimeOffset OccurredAt,
    string Actor,
    string Action,
    string EntityType,
    Guid EntityId,
    IReadOnlyDictionary<string, string> Details
);
