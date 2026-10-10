namespace Raccord.Application.Abstractions;

/// <summary>Commits all changes of a use case atomically.</summary>
public interface IUnitOfWork
{
    /// <summary>Persists pending changes in one transaction.</summary>
    /// <exception cref="ConcurrencyConflictException">Another operation changed the same records first.</exception>
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
