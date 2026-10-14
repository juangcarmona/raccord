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

    /// <summary>Lists configured task statuses.</summary>
    Task<IReadOnlyList<ProjectTaskStatus>> ListTaskStatusesAsync(
        CancellationToken cancellationToken
    );

    /// <summary>Finds a task status.</summary>
    Task<ProjectTaskStatus?> FindTaskStatusAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Lists project tasks.</summary>
    Task<IReadOnlyList<ProjectTask>> ListTasksAsync(
        Guid projectId,
        CancellationToken cancellationToken
    );

    /// <summary>Finds a task.</summary>
    Task<ProjectTask?> FindTaskAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Adds a task status.</summary>
    void Add(ProjectTaskStatus status);

    /// <summary>Adds a project task.</summary>
    void Add(ProjectTask task);
}
