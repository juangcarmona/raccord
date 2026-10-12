namespace Raccord.Modules.Compliance;

/// <summary>An append-only record of a sensitive or financial action.</summary>
public sealed class AuditEvent
{
    private AuditEvent()
    {
        Actor = string.Empty;
        Action = string.Empty;
        EntityType = string.Empty;
        Details = new Dictionary<string, string>(StringComparer.Ordinal);
    }

    /// <summary>Event identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Moment of the action.</summary>
    public DateTimeOffset OccurredAt { get; private set; }

    /// <summary>Who performed the action.</summary>
    public string Actor { get; private set; }

    /// <summary>Stable action code.</summary>
    public string Action { get; private set; }

    /// <summary>Affected entity type.</summary>
    public string EntityType { get; private set; }

    /// <summary>Affected entity.</summary>
    public Guid EntityId { get; private set; }

    /// <summary>Action-specific facts.</summary>
    public IReadOnlyDictionary<string, string> Details { get; private set; }

    /// <summary>Records an action.</summary>
    public static AuditEvent Record(
        DateTimeOffset occurredAt,
        string actor,
        string action,
        string entityType,
        Guid entityId,
        IReadOnlyDictionary<string, string> details
    ) =>
        new()
        {
            Id = Guid.CreateVersion7(occurredAt),
            OccurredAt = occurredAt,
            Actor = actor,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = new Dictionary<string, string>(details, StringComparer.Ordinal),
        };
}
