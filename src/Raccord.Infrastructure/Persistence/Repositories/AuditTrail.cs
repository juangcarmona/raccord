using Microsoft.EntityFrameworkCore;
using Raccord.Application.Auditing;
using Raccord.Modules.Compliance;
using Raccord.SharedKernel.Auditing;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class AuditTrail(RaccordDbContext db) : IAuditTrail
{
    public void Append(AuditEntry entry) =>
        db.AuditEvents.Add(
            AuditEvent.Record(
                entry.OccurredAt,
                entry.Actor,
                entry.Action,
                entry.EntityType,
                entry.EntityId,
                new Dictionary<string, string>(entry.Details, StringComparer.Ordinal)
            )
        );

    public async Task<
        IReadOnlyList<Raccord.Modules.Compliance.Contracts.AuditEventSnapshot>
    > ListForEntityAsync(Guid entityId, CancellationToken cancellationToken) =>
        await db
            .AuditEvents.AsNoTracking()
            .Where(item => item.EntityId == entityId)
            .OrderBy(item => item.OccurredAt)
            .ThenBy(item => item.Id)
            .Select(item => new Raccord.Modules.Compliance.Contracts.AuditEventSnapshot(
                item.Id,
                item.OccurredAt,
                item.Actor,
                item.Action,
                item.EntityType,
                item.EntityId,
                item.Details
            ))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
}
