using Raccord.SharedKernel.Auditing;

namespace Raccord.Modules.Compliance.Contracts;

/// <summary>Compliance-owned append-only audit trail.</summary>
public interface IAuditTrail : IAuditAppender
{
    /// <summary>Lists events for an entity in chronological order.</summary>
    Task<IReadOnlyList<AuditEventSnapshot>> ListForEntityAsync(
        Guid entityId,
        CancellationToken cancellationToken
    );
}
