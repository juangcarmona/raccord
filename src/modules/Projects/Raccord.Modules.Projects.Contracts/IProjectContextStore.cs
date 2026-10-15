using Raccord.Domain.Projects;

namespace Raccord.Modules.Projects.Contracts;

/// <summary>Persistence boundary for project working context.</summary>
public interface IProjectContextStore
{
    /// <summary>Lists project context records in creation order.</summary>
    Task<IReadOnlyList<ProjectContextEntry>> ListAsync(
        Guid projectId,
        CancellationToken cancellationToken
    );

    /// <summary>Adds a project context record.</summary>
    void Add(ProjectContextEntry entry);
}
