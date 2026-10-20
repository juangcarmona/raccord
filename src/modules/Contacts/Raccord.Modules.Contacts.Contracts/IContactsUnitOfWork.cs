namespace Raccord.Modules.Contacts.Contracts;

/// <summary>Persistence boundary used by Contacts use cases.</summary>
public interface IContactsUnitOfWork
{
    /// <summary>Persists the current use case atomically.</summary>
    Task SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>Runs the current Contacts use case and commits it atomically.</summary>
    Task<T> ExecuteAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken
    );
}
