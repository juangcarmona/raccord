using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Projects;

/// <summary>A configurable status used by project tasks.</summary>
public sealed class ProjectTaskStatus
{
    private ProjectTaskStatus()
    {
        Name = string.Empty;
    }

    private ProjectTaskStatus(string name, int position, bool completesProject)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        Position = position;
        CompletesProject = completesProject;
    }

    /// <summary>Identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Display name.</summary>
    public string Name { get; private set; }

    /// <summary>Board ordering position.</summary>
    public int Position { get; private set; }

    /// <summary>Whether tasks in this status count as complete.</summary>
    public bool CompletesProject { get; private set; }

    /// <summary>Creates a task status.</summary>
    public static ProjectTaskStatus Create(string name, int position, bool completesProject)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleViolationException(
                "projects.task-status-name-required",
                "A task status needs a name."
            );
        if (position < 0)
            throw new BusinessRuleViolationException(
                "projects.task-status-position-invalid",
                "A task status position cannot be negative."
            );
        return new ProjectTaskStatus(name.Trim(), position, completesProject);
    }
}
