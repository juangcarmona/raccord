using Microsoft.EntityFrameworkCore;
using Raccord.Modules.Projects.Contracts;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class ProjectScheduleRepository(RaccordDbContext db)
    : IProjectScheduleStore,
        IProjectSchedulePropagation
{
    public async Task PropagateRangeAsync(
        Guid projectId,
        Guid periodId,
        DateOnly? previousStart,
        DateOnly? previousEnd,
        DateOnly? nextStart,
        DateOnly? nextEnd,
        CancellationToken cancellationToken
    )
    {
        var linkedFunctions = await db
            .ProjectFunctions.Include(function => function.Shifts)
            .Where(function =>
                function.ProjectId == projectId && function.ProjectTimePeriodId == periodId
            )
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        if (linkedFunctions.Count == 0)
            return;
        if (previousStart is null || previousEnd is null || nextStart is null || nextEnd is null)
            throw new Raccord.SharedKernel.Common.BusinessRuleViolationException(
                "projects.linked-period-range-required",
                "A period linked to planning functions must keep a start and end date."
            );

        var startDelta = nextStart.Value.DayNumber - previousStart.Value.DayNumber;
        foreach (var function in linkedFunctions)
        {
            if (
                function.Shifts.Any(shift =>
                    shift.Date.AddDays(startDelta) < nextStart.Value
                    || shift.Date.AddDays(startDelta) > nextEnd.Value
                )
            )
                throw new Raccord.SharedKernel.Common.BusinessRuleViolationException(
                    "projects.linked-period-range-conflicts-with-shifts",
                    "The updated period would leave one or more linked shifts outside the period."
                );
            function.StartDate = nextStart.Value;
            function.EndDate = nextEnd.Value;
            foreach (var shift in function.Shifts)
                shift.Date = shift.Date.AddDays(startDelta);
        }
    }

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
