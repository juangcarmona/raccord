namespace Raccord.Modules.Projects.Contracts;

/// <summary>Coordinates updates to records linked to a project time period.</summary>
public interface IProjectSchedulePropagation
{
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
