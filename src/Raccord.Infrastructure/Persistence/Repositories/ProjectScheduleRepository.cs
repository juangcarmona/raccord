using Microsoft.EntityFrameworkCore;
using Raccord.Modules.Projects.Contracts;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class ProjectScheduleRepository(RaccordDbContext db) : IProjectScheduleStore
{
    public async Task ReorderAsync(
        Guid projectId,
        IReadOnlyList<Guid> orderedPeriodIds,
        CancellationToken cancellationToken
    )
    {
        await db
            .Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE projects.project_time_periods SET position = position + {int.MaxValue / 2} WHERE project_id = {projectId}",
                cancellationToken
            )
            .ConfigureAwait(false);
        for (var position = 0; position < orderedPeriodIds.Count; position++)
        {
            var periodId = orderedPeriodIds[position];
            await db
                .Database.ExecuteSqlInterpolatedAsync(
                    $"UPDATE projects.project_time_periods SET position = {position} WHERE project_id = {projectId} AND id = {periodId}",
                    cancellationToken
                )
                .ConfigureAwait(false);
        }
    }
}
