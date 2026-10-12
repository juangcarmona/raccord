using Raccord.Modules.Compliance.Contracts;

namespace Raccord.Modules.Compliance;

/// <summary>Read access to the compliance-owned audit trail.</summary>
public sealed class AuditLog(IAuditTrail trail)
{
    /// <summary>Lists the audit events of an entity in chronological order.</summary>
    public async Task<
        IReadOnlyList<Raccord.Contracts.Auditing.AuditEventResponse>
    > ListForEntityAsync(Guid entityId, CancellationToken cancellationToken) =>
        [
            .. (
                await trail.ListForEntityAsync(entityId, cancellationToken).ConfigureAwait(false)
            ).Select(item => new Raccord.Contracts.Auditing.AuditEventResponse(
                item.Id,
                item.OccurredAt,
                item.Actor,
                item.Action,
                item.EntityType,
                item.EntityId,
                item.Details
            )),
        ];
}
