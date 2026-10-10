namespace Raccord.Contracts.Auditing;

/// <summary>An append-only audit event.</summary>
/// <param name="Id">Event identifier.</param>
/// <param name="OccurredAt">Moment of the event (UTC).</param>
/// <param name="Actor">Who performed the action.</param>
/// <param name="Action">Stable action code, for example <c>invoice.issued</c>.</param>
/// <param name="EntityType">Affected entity type.</param>
/// <param name="EntityId">Affected entity.</param>
/// <param name="Details">Action-specific facts.</param>
public sealed record AuditEventResponse(
    Guid Id,
    DateTimeOffset OccurredAt,
    string Actor,
    string Action,
    string EntityType,
    Guid EntityId,
    IReadOnlyDictionary<string, string> Details);
