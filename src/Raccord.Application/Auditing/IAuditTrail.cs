using Raccord.Domain.Auditing;

namespace Raccord.Application.Auditing;

/// <summary>Append-only storage of audit events.</summary>
public interface IAuditTrail
{
    /// <summary>Appends an event; it is committed with the use case's unit of work.</summary>
    void Append(AuditEvent auditEvent);

    /// <summary>Lists events for an entity in chronological order.</summary>
    Task<IReadOnlyList<AuditEvent>> ListForEntityAsync(
        Guid entityId,
        CancellationToken cancellationToken
    );
}
