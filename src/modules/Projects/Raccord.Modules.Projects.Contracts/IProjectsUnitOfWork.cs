namespace Raccord.Modules.Projects.Contracts;

/// <summary>Persistence boundary used by Projects use cases.</summary>
public interface IProjectsUnitOfWork
{
    /// <summary>Persists all changes in the shared transaction.</summary>
    Task SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>Runs a project workflow and persists participating contexts atomically.</summary>
    Task<T> ExecuteAsync<T>(
        Func<CancellationToken, Task<T>> callback,
        CancellationToken cancellationToken
    );
}
