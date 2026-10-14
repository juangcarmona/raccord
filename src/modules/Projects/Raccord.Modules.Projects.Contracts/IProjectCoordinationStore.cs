using Raccord.Domain.Projects;

namespace Raccord.Modules.Projects.Contracts;

/// <summary>Persistence port for project-request coordination.</summary>
public interface IProjectCoordinationStore
{
    /// <summary>Lists incoming requests newest first.</summary>
    Task<IReadOnlyList<ProjectRequest>> ListRequestsAsync(CancellationToken cancellationToken);

    /// <summary>Finds one incoming request.</summary>
    Task<ProjectRequest?> FindRequestAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Lists configured project types.</summary>
    Task<IReadOnlyList<ProjectType>> ListProjectTypesAsync(CancellationToken cancellationToken);

    /// <summary>Lists configured project teams.</summary>
    Task<IReadOnlyList<ProjectTeam>> ListTeamsAsync(CancellationToken cancellationToken);

    /// <summary>Adds an incoming request.</summary>
    void Add(ProjectRequest request);

    /// <summary>Adds a project type.</summary>
    void Add(ProjectType projectType);

    /// <summary>Adds a project team.</summary>
    void Add(ProjectTeam team);
}
