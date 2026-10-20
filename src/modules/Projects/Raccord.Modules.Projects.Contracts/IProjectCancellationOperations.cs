namespace Raccord.Modules.Projects.Contracts;

/// <summary>Persistence port for cancellation actions owned by crew planning.</summary>
public interface IProjectCancellationOperations
{
    /// <summary>Counts assignments and active invitations for one project.</summary>
    Task<ProjectCancellationOperationsSummary> PreviewAsync(
        Guid projectId,
        CancellationToken cancellationToken
    );

    /// <summary>Applies only the selected crew planning cancellation actions.</summary>
    Task ApplyAsync(
        Guid projectId,
        bool removePlannedResources,
        bool withdrawInvitations,
        CancellationToken cancellationToken
    );
}
