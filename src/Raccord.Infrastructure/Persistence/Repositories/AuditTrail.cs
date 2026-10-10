using Microsoft.EntityFrameworkCore;
using Raccord.Application.Auditing;
using Raccord.Domain.Auditing;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class AuditTrail(RaccordDbContext db) : IAuditTrail
{
    public void Append(AuditEvent auditEvent) => db.AuditEvents.Add(auditEvent);

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
