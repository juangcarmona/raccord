using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Crew;
using Raccord.SharedKernel.Common;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable CS1591, MA0004

public sealed class CrewPlanningCatalog(RaccordDbContext db)
{
    public async Task<ProjectFunctionResponse?> CreateFunctionAsync(
        Guid projectId,
        CreateProjectFunctionRequest request,
        CancellationToken ct
    )
    {
        await EnsureOperationalProjectAsync(projectId, ct);
        if (
            string.IsNullOrWhiteSpace(request.Name)
            || request.RequiredResources < 1
            || request.EndDate < request.StartDate
        )
            throw new BusinessRuleViolationException(
                "crew.function-invalid",
                "A function needs a name, a valid period and at least one resource."
            );
        if (
            await db.ProjectFunctions.AnyAsync(
                row => row.ProjectId == projectId && row.Name == request.Name.Trim(),
                ct
            )
        )
            throw new BusinessRuleViolationException(
                "crew.function-duplicate",
                "A project cannot contain two functions with the same name."
            );
        var row = new ProjectFunctionRow
        {
            Id = Guid.CreateVersion7(),
            ProjectId = projectId,
            Name = request.Name.Trim(),
            Kind = request.Kind,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            RequiredResources = request.RequiredResources,
        };
        db.ProjectFunctions.Add(row);
        await db.SaveChangesAsync(ct);
        return ToResponse(row);
    }

    public async Task<IReadOnlyList<ProjectFunctionResponse>> ListFunctionsAsync(
        Guid projectId,
        CancellationToken ct
    ) =>
        (
            await db
                .ProjectFunctions.Include(row => row.Shifts)
                .Include(row => row.Crew)
                .Include(row => row.Vehicles)
                .Where(row => row.ProjectId == projectId)
                .OrderBy(row => row.StartDate)
                .ThenBy(row => row.Name)
                .ToListAsync(ct)
        )
            .Select(ToResponse)
            .ToArray();

    public async Task<IReadOnlyList<ProjectFunctionResponse>> GenerateFunctionsFromPeriodsAsync(
        Guid projectId,
        GenerateProjectFunctionsFromPeriodsRequest request,
        CancellationToken ct
    )
    {
        await EnsureOperationalProjectAsync(projectId, ct);
        ValidateGenerationRequest(request);
        var periods = await LoadSelectedPeriodsAsync(projectId, request.PeriodIds, ct);
        var generated = BuildFunctionsFromPeriods(projectId, periods, request.Kinds);
        var names = generated.Select(function => function.Name).ToArray();
        if (
            names.Distinct(StringComparer.Ordinal).Count() != names.Length
            || await db.ProjectFunctions.AnyAsync(
                function => function.ProjectId == projectId && names.Contains(function.Name),
                ct
            )
        )
            throw new BusinessRuleViolationException(
                "crew.period-generation-function-exists",
                "A function already exists for one of the selected periods and kinds."
            );

        db.ProjectFunctions.AddRange(generated);
        await db.SaveChangesAsync(ct);
        return generated.Select(ToResponse).ToArray();
    }

    private async Task<Raccord.Domain.Projects.ProjectTimePeriod[]> LoadSelectedPeriodsAsync(
        Guid projectId,
        IReadOnlyList<Guid> periodIds,
        CancellationToken ct
    )
    {
        var project = await db
            .Projects.Include(row => row.TimePeriods)
            .SingleOrDefaultAsync(row => row.Id == projectId, ct);
        if (project is null)
            throw new BusinessRuleViolationException("projects.not-found", "Project not found.");
        var periods = project.TimePeriods.Where(period => periodIds.Contains(period.Id)).ToArray();
        if (periods.Length != periodIds.Count)
            throw new BusinessRuleViolationException(
                "crew.period-generation-period-not-found",
                "Every selected period must belong to the project."
            );
        if (periods.Any(period => period.Start is null || period.End is null))
            throw new BusinessRuleViolationException(
                "crew.period-generation-period-untimed",
                "Set start and end times on every selected period before generating functions."
            );
        return periods;
    }

    private static void ValidateGenerationRequest(
        GenerateProjectFunctionsFromPeriodsRequest request
    )
    {
        if (
            request.PeriodIds.Count != 0
            && request.Kinds.Count != 0
            && request.PeriodIds.Distinct().Count() == request.PeriodIds.Count
            && request.Kinds.Distinct().Count() == request.Kinds.Count
            && request.Kinds.All(Enum.IsDefined)
        )
            return;
        throw new BusinessRuleViolationException(
            "crew.period-generation-invalid-selection",
            "Select one or more distinct periods and function kinds."
        );
    }

    private static ProjectFunctionRow[] BuildFunctionsFromPeriods(
        Guid projectId,
        IReadOnlyList<Raccord.Domain.Projects.ProjectTimePeriod> periods,
        IReadOnlyList<CrewFunctionKind> kinds
    ) =>
        periods
            .SelectMany(period =>
                kinds.Select(kind => new ProjectFunctionRow
                {
                    Id = Guid.CreateVersion7(),
                    ProjectId = projectId,
                    ProjectTimePeriodId = period.Id,
                    Name = $"{period.Name} — {kind}",
                    Kind = kind,
                    StartDate = DateOnly.FromDateTime(period.Start!.Value.DateTime),
                    EndDate = DateOnly.FromDateTime(period.End!.Value.DateTime),
                    RequiredResources = 1,
                })
            )
            .ToArray();

    public async Task<ProjectFunctionResponse?> AddShiftAsync(
        Guid functionId,
        CreateProjectShiftRequest request,
        CancellationToken ct
    )
    {
        var function = await Load(functionId, ct);
        if (function is null)
            return null;
        await EnsureOperationalProjectAsync(function.ProjectId, ct);
        if (
            request.EndTime <= request.StartTime
            || request.Date < function.StartDate
            || request.Date > function.EndDate
        )
            throw new BusinessRuleViolationException(
                "crew.shift-invalid",
                "A shift must have a positive duration within the function period."
            );
        function.Shifts.Add(
            new ProjectShiftRow
            {
                Id = Guid.CreateVersion7(),
                ProjectFunctionRowId = function.Id,
                Date = request.Date,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
            }
        );
        await db.SaveChangesAsync(ct);
        return ToResponse(function);
    }

    public async Task<ProjectFunctionResponse?> AssignCrewAsync(
        Guid functionId,
        AssignCrewRequest request,
        CancellationToken ct
    )
    {
        var function = await Load(functionId, ct);
        if (function is null)
            return null;
        await EnsureOperationalProjectAsync(function.ProjectId, ct);
        if (function.Kind != CrewFunctionKind.Crew)
            throw new BusinessRuleViolationException(
                "crew.function-kind-mismatch",
                "Crew members can only be assigned to crew functions."
            );
        if (!await db.CrewMembers.AnyAsync(row => row.Id == request.CrewMemberId, ct))
            throw new BusinessRuleViolationException(
                "crew.member-not-found",
                "The crew member does not exist."
            );
        if (function.Crew.Any(row => row.CrewMemberId == request.CrewMemberId))
            throw new BusinessRuleViolationException(
                "crew.assignment-duplicate",
                "The crew member is already assigned to this function."
            );
        function.Crew.Add(
            new CrewAssignmentRow
            {
                Id = Guid.CreateVersion7(),
                ProjectFunctionRowId = function.Id,
                CrewMemberId = request.CrewMemberId,
                RateId = request.RateId,
                IncludeTravel = request.IncludeTravel,
                IsProjectLeader = request.IsProjectLeader,
                Remark = Clean(request.Remark),
            }
        );
        await db.SaveChangesAsync(ct);
        return ToResponse(function);
    }

    public async Task<ProjectFunctionResponse?> AssignVehicleAsync(
        Guid functionId,
        AssignVehicleRequest request,
        CancellationToken ct
    )
    {
        var function = await Load(functionId, ct);
        if (function is null)
            return null;
        await EnsureOperationalProjectAsync(function.ProjectId, ct);
        if (function.Kind != CrewFunctionKind.Transport)
            throw new BusinessRuleViolationException(
                "crew.function-kind-mismatch",
                "Vehicles can only be assigned to transport functions."
            );
        if (!await db.Vehicles.AnyAsync(row => row.Id == request.VehicleId, ct))
            throw new BusinessRuleViolationException(
                "crew.vehicle-not-found",
                "The vehicle does not exist."
            );
        if (function.Vehicles.Any(row => row.VehicleId == request.VehicleId))
            throw new BusinessRuleViolationException(
                "crew.assignment-duplicate",
                "The vehicle is already assigned to this function."
            );
        function.Vehicles.Add(
            new VehicleAssignmentRow
            {
                Id = Guid.CreateVersion7(),
                ProjectFunctionRowId = function.Id,
                VehicleId = request.VehicleId,
                Remark = Clean(request.Remark),
            }
        );
        await db.SaveChangesAsync(ct);
        return ToResponse(function);
    }

    private Task<ProjectFunctionRow?> Load(Guid id, CancellationToken ct) =>
        db
            .ProjectFunctions.Include(row => row.Shifts)
            .Include(row => row.Crew)
            .Include(row => row.Vehicles)
            .SingleOrDefaultAsync(row => row.Id == id, ct);

    private async Task EnsureOperationalProjectAsync(Guid projectId, CancellationToken ct)
    {
        var status = await db
            .Projects.Where(project => project.Id == projectId)
            .Select(project => (Raccord.Domain.Projects.ProjectStatus?)project.Status)
            .SingleOrDefaultAsync(ct);
        if (status is null)
            throw new BusinessRuleViolationException("projects.not-found", "Project not found.");
        if (status == Raccord.Domain.Projects.ProjectStatus.Complete)
            throw new BusinessRuleViolationException(
                "projects.complete-read-only",
                "Reopen a completed project before changing its operational planning."
            );
    }

    private static string? Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static ProjectFunctionResponse ToResponse(ProjectFunctionRow row) =>
        new(
            row.Id,
            row.ProjectId,
            row.Name,
            row.Kind,
            row.StartDate,
            row.EndDate,
            row.RequiredResources,
            row.Kind == CrewFunctionKind.Crew ? row.Crew.Count : row.Vehicles.Count,
            (row.Kind == CrewFunctionKind.Crew ? row.Crew.Count : row.Vehicles.Count)
                >= row.RequiredResources,
            row.Shifts.Select(shift => new ProjectShiftResponse(
                    shift.Id,
                    shift.Date,
                    shift.StartTime,
                    shift.EndTime
                ))
                .ToArray(),
            row.Crew.Select(crew => new CrewAssignmentResponse(
                    crew.Id,
                    crew.CrewMemberId,
                    crew.RateId,
                    crew.IncludeTravel,
                    crew.IsProjectLeader,
                    crew.Remark
                ))
                .ToArray(),
            row.Vehicles.Select(vehicle => new VehicleAssignmentResponse(
                    vehicle.Id,
                    vehicle.VehicleId,
                    vehicle.Remark
                ))
                .ToArray(),
            row.ProjectTimePeriodId
        );
}

#pragma warning restore CS1591, MA0004
