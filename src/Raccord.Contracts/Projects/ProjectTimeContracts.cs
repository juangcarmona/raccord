namespace Raccord.Contracts.Projects;

#pragma warning disable CS1591

public sealed record RecordProjectTimeRequest(
    Guid? TaskId,
    string Service,
    decimal PlannedHours,
    decimal ActualHours,
    string Actor
);

public sealed record ProjectTimeEntryResponse(
    Guid Id,
    Guid? TaskId,
    string Service,
    decimal PlannedHours,
    decimal ActualHours,
    decimal VarianceHours,
    string Actor,
    DateTimeOffset RecordedAt
);

public sealed record ProjectTimeReportResponse(
    Guid ProjectId,
    decimal PlannedHours,
    decimal ActualHours,
    decimal VarianceHours,
    IReadOnlyList<ProjectTimeEntryResponse> Entries
);

#pragma warning restore CS1591
