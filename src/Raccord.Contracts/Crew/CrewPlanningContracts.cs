#pragma warning disable CS1591

namespace Raccord.Contracts.Crew;

public enum CrewFunctionKind
{
    Crew,
    Transport,
}

public sealed record CreateProjectFunctionRequest(
    string Name,
    CrewFunctionKind Kind,
    DateOnly StartDate,
    DateOnly EndDate,
    int RequiredResources = 1
);

public sealed record GenerateProjectFunctionsFromPeriodsRequest(
    IReadOnlyList<Guid> PeriodIds,
    IReadOnlyList<CrewFunctionKind> Kinds
);

public sealed record CreateProjectShiftRequest(DateOnly Date, TimeOnly StartTime, TimeOnly EndTime);

public sealed record AssignCrewRequest(
    Guid CrewMemberId,
    Guid? RateId = null,
    bool IncludeTravel = false,
    bool IsProjectLeader = false,
    string? Remark = null
);

public sealed record AssignVehicleRequest(Guid VehicleId, string? Remark = null);

public sealed record ProjectShiftResponse(
    Guid Id,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime
);

public sealed record CrewAssignmentResponse(
    Guid Id,
    Guid CrewMemberId,
    Guid? RateId,
    bool IncludeTravel,
    bool IsProjectLeader,
    string? Remark
);

public sealed record VehicleAssignmentResponse(Guid Id, Guid VehicleId, string? Remark);

public sealed record ProjectFunctionResponse(
    Guid Id,
    Guid ProjectId,
    string Name,
    CrewFunctionKind Kind,
    DateOnly StartDate,
    DateOnly EndDate,
    int RequiredResources,
    int AssignedResources,
    bool IsStaffed,
    IReadOnlyList<ProjectShiftResponse> Shifts,
    IReadOnlyList<CrewAssignmentResponse> Crew,
    IReadOnlyList<VehicleAssignmentResponse> Vehicles,
    Guid? ProjectTimePeriodId = null
);

#pragma warning restore CS1591
