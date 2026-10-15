using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Projects;

#pragma warning disable CS1591

/// <summary>An append-only planned-versus-actual time record for a project.</summary>
public sealed class ProjectTimeEntry
{
    private ProjectTimeEntry() { }

    private ProjectTimeEntry(
        Guid projectId,
        Guid? taskId,
        string service,
        decimal plannedHours,
        decimal actualHours,
        string actor
    )
    {
        Id = Guid.CreateVersion7();
        ProjectId = projectId;
        TaskId = taskId;
        Service = service;
        PlannedHours = plannedHours;
        ActualHours = actualHours;
        Actor = actor;
        RecordedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid? TaskId { get; private set; }
    public string Service { get; private set; } = string.Empty;
    public decimal PlannedHours { get; private set; }
    public decimal ActualHours { get; private set; }
    public string Actor { get; private set; } = string.Empty;
    public DateTimeOffset RecordedAt { get; private set; }

    public static ProjectTimeEntry Record(
        Guid projectId,
        Guid? taskId,
        string service,
        decimal plannedHours,
        decimal actualHours,
        string actor
    )
    {
        if (projectId == Guid.Empty)
            throw new BusinessRuleViolationException(
                "projects.time-project-required",
                "A project is required."
            );
        if (taskId is Guid task && task == Guid.Empty)
            throw new BusinessRuleViolationException(
                "projects.time-task-invalid",
                "The task identifier is invalid."
            );
        if (string.IsNullOrWhiteSpace(service))
            throw new BusinessRuleViolationException(
                "projects.time-service-required",
                "A task or service is required."
            );
        if (plannedHours < 0m || actualHours < 0m)
            throw new BusinessRuleViolationException(
                "projects.time-hours-invalid",
                "Time values cannot be negative."
            );
        if (string.IsNullOrWhiteSpace(actor))
            throw new BusinessRuleViolationException(
                "projects.time-actor-required",
                "An actor is required."
            );
        return new(projectId, taskId, service.Trim(), plannedHours, actualHours, actor.Trim());
    }
}

#pragma warning restore CS1591
