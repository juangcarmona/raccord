using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Projects;

/// <summary>A scheduled work item for a project and operator.</summary>
public sealed class ProjectSchedule
{
    private ProjectSchedule()
    {
        Title = string.Empty;
    }

    private ProjectSchedule(
        Guid projectId,
        string title,
        DateOnly startDate,
        DateOnly endDate,
        Guid? assigneeId
    )
    {
        Id = Guid.CreateVersion7();
        ProjectId = projectId;
        Title = title;
        StartDate = startDate;
        EndDate = endDate;
        AssigneeId = assigneeId;
    }

    /// <summary>Identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Owning project.</summary>
    public Guid ProjectId { get; private set; }

    /// <summary>Work item title.</summary>
    public string Title { get; private set; }

    /// <summary>First scheduled day.</summary>
    public DateOnly StartDate { get; private set; }

    /// <summary>Last scheduled day.</summary>
    public DateOnly EndDate { get; private set; }

    /// <summary>Assigned operator, when known.</summary>
    public Guid? AssigneeId { get; private set; }

    /// <summary>Creates a scheduled work item.</summary>
    public static ProjectSchedule Create(
        Guid projectId,
        string title,
        DateOnly startDate,
        DateOnly endDate,
        Guid? assigneeId
    )
    {
        if (projectId == Guid.Empty)
            throw new BusinessRuleViolationException(
                "projects.schedule-project-required",
                "A schedule needs a project."
            );
        if (string.IsNullOrWhiteSpace(title))
            throw new BusinessRuleViolationException(
                "projects.schedule-title-required",
                "A schedule needs a title."
            );
        if (endDate < startDate)
            throw new BusinessRuleViolationException(
                "projects.schedule-period-invalid",
                "A schedule cannot end before it starts."
            );
        return new ProjectSchedule(projectId, title.Trim(), startDate, endDate, assigneeId);
    }

    /// <summary>Shifts the schedule by whole days.</summary>
    public void Shift(int days)
    {
        StartDate = StartDate.AddDays(days);
        EndDate = EndDate.AddDays(days);
    }
}
