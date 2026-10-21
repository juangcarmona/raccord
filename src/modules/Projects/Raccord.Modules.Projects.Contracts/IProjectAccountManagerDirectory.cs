namespace Raccord.Modules.Projects.Contracts;

/// <summary>Reads workspace users eligible to manage a project.</summary>
public interface IProjectAccountManagerDirectory
{
    /// <summary>Lists Admin and Office users.</summary>
    Task<IReadOnlyList<ProjectAccountManagerSnapshot>> ListAsync(
        CancellationToken cancellationToken
    );

    /// <summary>Returns whether a user has the Admin or Office role.</summary>
    Task<bool> IsEligibleAsync(Guid userId, CancellationToken cancellationToken);
}
