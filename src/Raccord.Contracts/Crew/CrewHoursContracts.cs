#pragma warning disable CS1591
namespace Raccord.Contracts.Crew;

public enum CrewHourSource
{
    Manual,
    PlannedActivity,
}

public enum CrewHourStatus
{
    Pending,
    Approved,
    Rejected,
}

public sealed record RegisterCrewHoursRequest(
    Guid CrewMemberId,
    DateOnly WorkDate,
    decimal Hours,
    CrewHourSource Source = CrewHourSource.Manual,
    Guid? FunctionId = null,
    string? Remark = null
);

public sealed record ApproveCrewHoursRequest(CrewHourStatus Status);

public sealed record CrewHoursResponse(
    Guid Id,
    Guid CrewMemberId,
    DateOnly WorkDate,
    decimal Hours,
    CrewHourSource Source,
    Guid? FunctionId,
    CrewHourStatus Status,
    string? Remark
);
#pragma warning restore CS1591
