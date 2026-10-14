using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Projects;

/// <summary>Reusable team assignment for production projects.</summary>
public sealed class ProjectTeam
{
    private ProjectTeam() { }

    private ProjectTeam(string name, string? description)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        Description = description;
    }

    /// <summary>Team identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Team name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Optional team description.</summary>
    public string? Description { get; private set; }

    /// <summary>Creates a project team.</summary>
    public static ProjectTeam Create(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleViolationException(
                "projects.team-name-required",
                "Team name is required."
            );
        return new ProjectTeam(
            name.Trim(),
            string.IsNullOrWhiteSpace(description) ? null : description.Trim()
        );
    }
}
