using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Organization;

/// <summary>Company-wide commercial settings used by future quote and contract workflows.</summary>
public sealed class FinancialSettings
{
    private FinancialSettings() { }

    private FinancialSettings(
        int quoteValidityDays,
        int contractValidityDays,
        bool allowSigningAfterExpiry
    )
    {
        Id = Guid.CreateVersion7();
        Update(quoteValidityDays, contractValidityDays, allowSigningAfterExpiry);
    }

    /// <summary>Settings identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Default quote validity in days.</summary>
    public int QuoteValidityDays { get; private set; }

    /// <summary>Default contract validity in days.</summary>
    public int ContractValidityDays { get; private set; }

    /// <summary>Whether signing after expiry is permitted.</summary>
    public bool AllowSigningAfterExpiry { get; private set; }

    /// <summary>Creates validated financial settings.</summary>
    public static FinancialSettings Configure(
        int quoteValidityDays,
        int contractValidityDays,
        bool allowSigningAfterExpiry
    )
    {
        var settings = new FinancialSettings();
        settings.Id = Guid.CreateVersion7();
        settings.Update(quoteValidityDays, contractValidityDays, allowSigningAfterExpiry);
        return settings;
    }

    /// <summary>Updates the validated financial settings.</summary>
    public void Update(
        int quoteValidityDays,
        int contractValidityDays,
        bool allowSigningAfterExpiry
    )
    {
        if (quoteValidityDays <= 0 || contractValidityDays <= 0)
            throw new BusinessRuleViolationException(
                "financial-settings.validity-positive",
                "Quote and contract validity must be greater than zero days."
            );

        QuoteValidityDays = quoteValidityDays;
        ContractValidityDays = contractValidityDays;
        AllowSigningAfterExpiry = allowSigningAfterExpiry;
    }
}
