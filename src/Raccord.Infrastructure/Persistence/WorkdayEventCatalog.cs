using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Crew;
using Raccord.SharedKernel.Common;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable CS1591, MA0004
public sealed class WorkdayEventCatalog(RaccordDbContext db)
{
    public async Task<WorkdayEventResponse> RecordAsync(
        RecordWorkdayEventRequest request,
        CancellationToken ct
    )
    {
        if (string.IsNullOrWhiteSpace(request.Actor))
            throw new BusinessRuleViolationException(
                "crew.workday-actor-required",
                "An actor is required to record a workday event."
            );
        if (request.OccurredAt > DateTimeOffset.UtcNow.AddMinutes(5))
            throw new BusinessRuleViolationException(
                "crew.workday-future-event",
                "A workday event cannot be recorded far in the future."
            );
        var row = new WorkdayEventRow
        {
            Id = Guid.CreateVersion7(),
            CrewMemberId = request.CrewMemberId,
            Kind = request.Kind,
            OccurredAt = request.OccurredAt,
            Actor = request.Actor.Trim(),
            RecordedAt = DateTimeOffset.UtcNow,
            Remark = request.Remark?.Trim(),
        };
        db.WorkdayEvents.Add(row);
        await db.SaveChangesAsync(ct);
        return ToResponse(row);
    }

    public async Task<IReadOnlyList<WorkdayEventResponse>> ListAsync(
        Guid crewMemberId,
        CancellationToken ct
    ) =>
        (
            await db
                .WorkdayEvents.Where(row => row.CrewMemberId == crewMemberId)
                .OrderBy(row => row.OccurredAt)
                .ToListAsync(ct)
        )
            .Select(ToResponse)
            .ToList();

    private static WorkdayEventResponse ToResponse(WorkdayEventRow row) =>
        new(
            row.Id,
            row.CrewMemberId,
            row.Kind,
            row.OccurredAt,
            row.Actor,
            row.RecordedAt,
            row.Remark
        );
}
#pragma warning restore CS1591, MA0004
