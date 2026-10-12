using Raccord.Domain.Projects;

namespace Raccord.Modules.Projects.Contracts;

/// <summary>Persistence port used by Projects and consumed by Invoicing.</summary>
public interface IBillableWork
{
    /// <summary>Finds a project with its billable lines.</summary>
    Task<Project?> FindAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Lists projects with their billable lines.</summary>
    Task<IReadOnlyList<Project>> ListAsync(CancellationToken cancellationToken);

    /// <summary>Adds a project.</summary>
    void Add(Project project);
}
