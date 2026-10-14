using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Projects;

/// <summary>Reusable project classification.</summary>
public sealed class ProjectType
{
    private ProjectType() { }

    private ProjectType(string name, string? description)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        Description = description;
    }

    /// <summary>Type identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Type name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Optional type description.</summary>
    public string? Description { get; private set; }

    /// <summary>Creates a project type.</summary>
    public static ProjectType Create(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleViolationException(
                "projects.type-name-required",
                "Project type name is required."
            );
        return new ProjectType(
            name.Trim(),
            string.IsNullOrWhiteSpace(description) ? null : description.Trim()
        );
    }
}
