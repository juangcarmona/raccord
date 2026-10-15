using Raccord.Domain.Projects;

namespace Raccord.Modules.Projects.Contracts;

/// <summary>Persistence boundary for append-only project time records.</summary>
public interface IProjectTimeStore
{
    /// <summary>Lists time records for a project in recording order.</summary>
    Task<IReadOnlyList<ProjectTimeEntry>> ListAsync(
        Guid projectId,
        CancellationToken cancellationToken
    );

    /// <summary>Adds a time record.</summary>
    void Add(ProjectTimeEntry entry);
}
