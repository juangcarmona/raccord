using Raccord.Domain.Organization;

namespace Raccord.Application.Organization;

/// <summary>Persistence of the issuer profile (one per installation).</summary>
public interface IIssuerProfileRepository
{
    /// <summary>Finds the issuer profile, if defined.</summary>
    Task<IssuerProfile?> FindAsync(CancellationToken cancellationToken);

    /// <summary>Adds the issuer profile.</summary>
    void Add(IssuerProfile profile);
}
