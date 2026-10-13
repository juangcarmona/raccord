using Raccord.Domain.Organization;

namespace Raccord.Application.Organization;

/// <summary>Persistence port for the one company-wide financial settings row.</summary>
public interface IFinancialSettingsRepository
{
    /// <summary>Loads the single company-wide settings row.</summary>
    Task<FinancialSettings?> FindAsync(CancellationToken cancellationToken);

    /// <summary>Adds the settings row.</summary>
    void Add(FinancialSettings settings);
}
