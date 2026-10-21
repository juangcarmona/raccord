namespace Raccord.Modules.Projects.Contracts;

/// <summary>Serializes and allocates installation-wide project numbers.</summary>
public interface IProjectNumberSeries
{
    /// <summary>Acquires the transaction-scoped lock shared by every number assignment.</summary>
    Task LockAsync(CancellationToken cancellationToken);

    /// <summary>Returns the next number not already assigned to a project.</summary>
    Task<string> NextAsync(CancellationToken cancellationToken);

    /// <summary>Returns whether the number is already assigned.</summary>
    Task<bool> IsAssignedAsync(
        string number,
        Guid? excludingProjectId,
        CancellationToken cancellationToken
    );
}
