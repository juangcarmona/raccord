using Microsoft.EntityFrameworkCore;
using Raccord.Application.Auditing;
using Raccord.Domain.Auditing;
using Raccord.SharedKernel.Auditing;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class AuditTrail(RaccordDbContext db) : IAuditTrail
{
    public void Append(AuditEvent auditEvent) => db.AuditEvents.Add(auditEvent);

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

    public async Task<IReadOnlyList<AuditEvent>> ListForEntityAsync(
        Guid entityId,
        CancellationToken cancellationToken
    ) =>
        await db
            .AuditEvents.AsNoTracking()
            .Where(item => item.EntityId == entityId)
            .OrderBy(item => item.OccurredAt)
            .ThenBy(item => item.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
}
