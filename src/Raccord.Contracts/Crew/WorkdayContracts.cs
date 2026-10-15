#pragma warning disable CS1591
namespace Raccord.Contracts.Crew;

public enum WorkdayEventKind
{
    ClockIn,
    ClockOut,
}

public sealed record RecordWorkdayEventRequest(
    Guid CrewMemberId,
    WorkdayEventKind Kind,
    DateTimeOffset OccurredAt,
    string Actor,
    string? Remark = null
);

public sealed record WorkdayEventResponse(
    Guid Id,
    Guid CrewMemberId,
    WorkdayEventKind Kind,
    DateTimeOffset OccurredAt,
    string Actor,
    DateTimeOffset RecordedAt,
    string? Remark
);
#pragma warning restore CS1591
