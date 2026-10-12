namespace Raccord.SharedKernel.Auditing;

/// <summary>An immutable audit fact accepted by the shared audit port.</summary>
public sealed record AuditEntry(
    DateTimeOffset OccurredAt,
    string Actor,
    string Action,
    string EntityType,
    Guid EntityId,
    IReadOnlyDictionary<string, string> Details
);
