using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Projects;

/// <summary>A task shown on a project's shared board.</summary>
public sealed class ProjectTask
{
    private ProjectTask()
    {
        Title = string.Empty;
    }

    private ProjectTask(
        Guid projectId,
        string title,
        string? description,
        Guid statusId,
        int position,
        Guid? assigneeId
    )
    {
        Id = Guid.CreateVersion7();
        ProjectId = projectId;
        Title = title;
        Description = description;
        StatusId = statusId;
        Position = position;
        AssigneeId = assigneeId;
    }

    /// <summary>Identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Owning project.</summary>
    public Guid ProjectId { get; private set; }

    /// <summary>Task title.</summary>
    public string Title { get; private set; }

    /// <summary>Optional task details.</summary>
    public string? Description { get; private set; }

    /// <summary>Configured status identifier.</summary>
    public Guid StatusId { get; private set; }

    /// <summary>Ordering inside a status column.</summary>
    public int Position { get; private set; }

    /// <summary>Assigned operator.</summary>
    public Guid? AssigneeId { get; private set; }

    /// <summary>Creates a project task.</summary>
    public static ProjectTask Create(
        Guid projectId,
        string title,
        string? description,
        Guid statusId,
        int position,
        Guid? assigneeId
    )
    {
        if (projectId == Guid.Empty)
            throw new BusinessRuleViolationException(
                "projects.task-project-required",
                "A task needs a project."
            );
        if (string.IsNullOrWhiteSpace(title))
            throw new BusinessRuleViolationException(
                "projects.task-title-required",
                "A task needs a title."
            );
        if (position < 0)
            throw new BusinessRuleViolationException(
                "projects.task-position-invalid",
                "A task position cannot be negative."
            );
        return new ProjectTask(
            projectId,
            title.Trim(),
            string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            statusId,
            position,
            assigneeId
        );
    }

    /// <summary>Moves the task to another status and board position.</summary>
    public void Progress(Guid statusId, int position)
    {
        if (statusId == Guid.Empty)
            throw new BusinessRuleViolationException(
                "projects.task-status-required",
                "A task needs a status."
            );
        if (position < 0)
            throw new BusinessRuleViolationException(
                "projects.task-position-invalid",
                "A task position cannot be negative."
            );
        StatusId = statusId;
        Position = position;
    }
}
