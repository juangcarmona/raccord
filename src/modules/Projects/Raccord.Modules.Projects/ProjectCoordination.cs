using Raccord.Contracts.Projects;
using Raccord.Domain.Projects;
using Raccord.Modules.Contacts.Contracts;
using Raccord.Modules.Projects.Contracts;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Modules.Projects;

/// <summary>Coordinates incoming project requests and reusable classifications.</summary>
public sealed class ProjectCoordination(
    IProjectCoordinationStore coordination,
    IBillableWork projects,
    IPartyDirectory parties,
    IProjectsUnitOfWork unitOfWork
)
{
    /// <summary>Lists incoming project requests.</summary>
    public async Task<IReadOnlyList<ProjectRequestResponse>> ListRequestsAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (
                await coordination.ListRequestsAsync(cancellationToken).ConfigureAwait(false)
            ).Select(ToResponse),
        ];

    /// <summary>Records an incoming project request.</summary>
    public async Task<ProjectRequestResponse> ReceiveAsync(
        ReceiveProjectRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        await ValidateCustomerAsync(request.CustomerId, cancellationToken).ConfigureAwait(false);
        var projectRequest = ProjectRequest.Receive(
            request.Name,
            request.RequesterName,
            request.RequesterEmail,
            request.CustomerId,
            request.Description,
            request.StartDate,
            request.EndDate,
            request.ProjectTypeId,
            request.TeamId
        );
        coordination.Add(projectRequest);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(projectRequest);
    }

    /// <summary>Converts a request into a project and retains its source record.</summary>
    public async Task<ProjectResponse?> ConvertAsync(
        Guid requestId,
        CancellationToken cancellationToken
    )
    {
        var request = await coordination
            .FindRequestAsync(requestId, cancellationToken)
            .ConfigureAwait(false);
        if (request is null)
            return null;
        if (request.CustomerId is not Guid customerId)
            throw new BusinessRuleViolationException(
                "projects.request-customer-required",
                "A customer must be linked before converting the request."
            );
        var customer = await parties.FindAsync(customerId, cancellationToken).ConfigureAwait(false);
        if (customer is null)
            throw new BusinessRuleViolationException(
                "customers.not-found",
                $"Customer '{customerId}' does not exist."
            );
        var project = Project.Create(
            request.Name,
            customerId,
            request.StartDate,
            request.EndDate,
            request.ProjectTypeId,
            request.TeamId
        );
        projects.Add(project);
        request.Convert(project.Id);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return new ProjectResponse(
            project.Id,
            project.Name,
            project.CustomerId,
            customer.LegalName,
            project.StartDate,
            project.EndDate,
            Currency.Eur.Code,
            [],
            project.ProjectTypeId,
            project.TeamId
        );
    }

    /// <summary>Declines an incoming request.</summary>
    public async Task<ProjectRequestResponse?> DeclineAsync(
        Guid requestId,
        DeclineProjectRequest input,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(input);
        var request = await coordination
            .FindRequestAsync(requestId, cancellationToken)
            .ConfigureAwait(false);
        if (request is null)
            return null;
        request.Decline(input.Reason);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(request);
    }

    /// <summary>Creates a reusable project type.</summary>
    public async Task<ProjectTypeResponse> CreateTypeAsync(
        CreateProjectClassificationRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var projectType = ProjectType.Create(request.Name, request.Description);
        coordination.Add(projectType);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return new(projectType.Id, projectType.Name, projectType.Description);
    }

    /// <summary>Creates a reusable project team.</summary>
    public async Task<ProjectTeamResponse> CreateTeamAsync(
        CreateProjectClassificationRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var team = ProjectTeam.Create(request.Name, request.Description);
        coordination.Add(team);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return new(team.Id, team.Name, team.Description);
    }

    /// <summary>Lists reusable project types.</summary>
    public async Task<IReadOnlyList<ProjectTypeResponse>> ListTypesAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (
                await coordination.ListProjectTypesAsync(cancellationToken).ConfigureAwait(false)
            ).Select(type => new ProjectTypeResponse(type.Id, type.Name, type.Description)),
        ];

    /// <summary>Lists reusable project teams.</summary>
    public async Task<IReadOnlyList<ProjectTeamResponse>> ListTeamsAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (await coordination.ListTeamsAsync(cancellationToken).ConfigureAwait(false)).Select(
                team => new ProjectTeamResponse(team.Id, team.Name, team.Description)
            ),
        ];

    /// <summary>Creates a configurable task status.</summary>
    public async Task<ProjectTaskStatusResponse> CreateTaskStatusAsync(
        CreateProjectTaskStatusRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var status = ProjectTaskStatus.Create(
            request.Name,
            request.Position,
            request.CompletesProject
        );
        coordination.Add(status);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(status);
    }

    /// <summary>Lists configurable task statuses.</summary>
    public async Task<IReadOnlyList<ProjectTaskStatusResponse>> ListTaskStatusesAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (
                await coordination.ListTaskStatusesAsync(cancellationToken).ConfigureAwait(false)
            ).Select(ToResponse),
        ];

    /// <summary>Creates a task on a project board.</summary>
    public async Task<ProjectTaskResponse> CreateTaskAsync(
        CreateProjectTaskRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        await EnsureProjectAsync(request.ProjectId, cancellationToken).ConfigureAwait(false);
        await EnsureStatusAsync(request.StatusId, cancellationToken).ConfigureAwait(false);
        var task = ProjectTask.Create(
            request.ProjectId,
            request.Title,
            request.Description,
            request.StatusId,
            request.Position,
            request.AssigneeId
        );
        coordination.Add(task);
        await RecalculateProgressAsync(request.ProjectId, cancellationToken).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(task);
    }

    /// <summary>Returns the project board.</summary>
    public async Task<ProjectBoardResponse?> GetBoardAsync(
        Guid projectId,
        CancellationToken cancellationToken
    )
    {
        var project = await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false);
        if (project is null)
            return null;
        var tasks = await coordination
            .ListTasksAsync(projectId, cancellationToken)
            .ConfigureAwait(false);
        return new(projectId, project.ProgressPercent, [.. tasks.Select(ToResponse)]);
    }

    /// <summary>Moves a task and recalculates project progress.</summary>
    public async Task<ProjectTaskResponse?> ProgressTaskAsync(
        Guid taskId,
        ProgressProjectTaskRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var task = await coordination
            .FindTaskAsync(taskId, cancellationToken)
            .ConfigureAwait(false);
        if (task is null)
            return null;
        await EnsureStatusAsync(request.StatusId, cancellationToken).ConfigureAwait(false);
        task.Progress(request.StatusId, request.Position);
        await RecalculateProgressAsync(task.ProjectId, cancellationToken).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(task);
    }

    private async Task EnsureProjectAsync(Guid projectId, CancellationToken cancellationToken)
    {
        if (await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false) is null)
            throw new BusinessRuleViolationException(
                "projects.not-found",
                $"Project '{projectId}' does not exist."
            );
    }

    private async Task EnsureStatusAsync(Guid statusId, CancellationToken cancellationToken)
    {
        if (
            await coordination
                .FindTaskStatusAsync(statusId, cancellationToken)
                .ConfigureAwait(false)
            is null
        )
            throw new BusinessRuleViolationException(
                "projects.task-status-not-found",
                $"Task status '{statusId}' does not exist."
            );
    }

    private async Task RecalculateProgressAsync(Guid projectId, CancellationToken cancellationToken)
    {
        var project = await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false);
        if (project is null)
            return;
        var tasks = await coordination
            .ListTasksAsync(projectId, cancellationToken)
            .ConfigureAwait(false);
        var statuses = await coordination
            .ListTaskStatusesAsync(cancellationToken)
            .ConfigureAwait(false);
        var completed =
            tasks.Count == 0
                ? 0
                : tasks.Count(task =>
                    statuses.FirstOrDefault(status => status.Id == task.StatusId)?.CompletesProject
                    == true
                )
                    * 100m
                    / tasks.Count;
        project.UpdateProgress(completed);
    }

    private static ProjectTaskStatusResponse ToResponse(ProjectTaskStatus status) =>
        new(status.Id, status.Name, status.Position, status.CompletesProject);

    private static ProjectTaskResponse ToResponse(ProjectTask task) =>
        new(
            task.Id,
            task.ProjectId,
            task.Title,
            task.Description,
            task.StatusId,
            task.Position,
            task.AssigneeId
        );

    /// <summary>Creates scheduled project work.</summary>
    public async Task<ProjectScheduleResponse> CreateScheduleAsync(
        CreateProjectScheduleRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        if (
            await projects.FindAsync(request.ProjectId, cancellationToken).ConfigureAwait(false)
            is null
        )
            throw new BusinessRuleViolationException(
                "projects.not-found",
                $"Project '{request.ProjectId}' does not exist."
            );
        var schedule = ProjectSchedule.Create(
            request.ProjectId,
            request.Title,
            request.StartDate,
            request.EndDate,
            request.AssigneeId
        );
        coordination.Add(schedule);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(schedule);
    }

    /// <summary>Lists project schedules, optionally for one operator.</summary>
    public async Task<IReadOnlyList<ProjectScheduleResponse>> ListSchedulesAsync(
        Guid? projectId,
        Guid? assigneeId,
        CancellationToken cancellationToken
    ) =>
        [
            .. (
                await coordination
                    .ListSchedulesAsync(projectId, assigneeId, cancellationToken)
                    .ConfigureAwait(false)
            ).Select(ToResponse),
        ];

    /// <summary>Shifts a schedule by whole days.</summary>
    public async Task<ProjectScheduleResponse?> ShiftScheduleAsync(
        Guid id,
        ShiftProjectScheduleRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var schedule = await coordination
            .FindScheduleAsync(id, cancellationToken)
            .ConfigureAwait(false);
        if (schedule is null)
            return null;
        schedule.Shift(request.Days);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(schedule);
    }

    private static ProjectScheduleResponse ToResponse(ProjectSchedule schedule) =>
        new(
            schedule.Id,
            schedule.ProjectId,
            schedule.Title,
            schedule.StartDate,
            schedule.EndDate,
            schedule.AssigneeId
        );

    private async Task ValidateCustomerAsync(Guid? customerId, CancellationToken cancellationToken)
    {
        if (
            customerId is Guid id
            && await parties.FindAsync(id, cancellationToken).ConfigureAwait(false) is null
        )
            throw new BusinessRuleViolationException(
                "customers.not-found",
                $"Customer '{id}' does not exist."
            );
    }

    private static ProjectRequestResponse ToResponse(ProjectRequest request) =>
        new(
            request.Id,
            request.Name,
            request.RequesterName,
            request.RequesterEmail,
            request.CustomerId,
            request.Description,
            request.StartDate,
            request.EndDate,
            request.ProjectTypeId,
            request.TeamId,
            request.Status.ToString().ToLowerInvariant(),
            request.ProjectId,
            request.DeclineReason,
            request.ReceivedAt,
            request.ResolvedAt
        );
}
