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

    /// <summary>Moves linked planning with its period, preserving shift offsets where possible.</summary>
    Task PropagateRangeAsync(
        Guid projectId,
        Guid periodId,
        DateOnly? previousStart,
        DateOnly? previousEnd,
        DateOnly? nextStart,
        DateOnly? nextEnd,
        CancellationToken cancellationToken
    );
}
