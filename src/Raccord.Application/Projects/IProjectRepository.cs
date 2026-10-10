using Raccord.Domain.Projects;

namespace Raccord.Application.Projects;

/// <summary>Persistence of projects and their billable lines.</summary>
public interface IProjectRepository
{
    /// <summary>Finds a project with its billable lines.</summary>
    Task<Project?> FindAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Lists projects with their billable lines, most recent first.</summary>
    Task<IReadOnlyList<Project>> ListAsync(CancellationToken cancellationToken);

    /// <summary>Adds a new project.</summary>
    void Add(Project project);
}
