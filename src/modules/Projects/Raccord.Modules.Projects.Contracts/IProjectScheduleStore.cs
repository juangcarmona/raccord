namespace Raccord.Modules.Projects.Contracts;

/// <summary>Persistence boundary for project schedule ordering.</summary>
public interface IProjectScheduleStore
{
    /// <summary>Reorders all project periods inside the caller's transaction.</summary>
    Task ReorderAsync(
        Guid projectId,
        IReadOnlyList<Guid> orderedPeriodIds,
        CancellationToken cancellationToken
    );
}
