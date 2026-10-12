namespace Raccord.BuildingBlocks.Persistence;

/// <summary>Coordinates a shared database transaction across participating contexts.</summary>
public interface IUnitOfWork
{
    /// <summary>Saves the registered contexts atomically.</summary>
    Task SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>Runs a callback and commits all participating contexts atomically.</summary>
    Task<T> ExecuteAsync<T>(
        Func<CancellationToken, Task<T>> callback,
        CancellationToken cancellationToken
    );
}
