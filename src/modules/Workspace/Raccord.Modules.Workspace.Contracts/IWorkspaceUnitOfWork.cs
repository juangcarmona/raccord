namespace Raccord.Modules.Workspace.Contracts;

/// <summary>Persistence boundary used by Workspace use cases.</summary>
public interface IWorkspaceUnitOfWork
{
    /// <summary>Persists the current use case atomically.</summary>
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
