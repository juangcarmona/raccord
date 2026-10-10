using Raccord.Contracts.Auditing;

namespace Raccord.Application.Auditing;

/// <summary>Read access to the audit trail.</summary>
public sealed class AuditLog(IAuditTrail trail)
{
    /// <summary>Lists the audit events of an entity in chronological order.</summary>
    public async Task<IReadOnlyList<AuditEventResponse>> ListForEntityAsync(
        Guid entityId,
        CancellationToken cancellationToken
    ) =>
        [
            .. (
                await trail.ListForEntityAsync(entityId, cancellationToken).ConfigureAwait(false)
            ).Select(item => new AuditEventResponse(
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
