using Raccord.Domain.Common;

namespace Raccord.Domain.Pricing;

/// <summary>A value-added-tax rate expressed as a percentage, for example 21.</summary>
public readonly record struct VatRate
{
    private VatRate(decimal percentage)
    {
        Percentage = percentage;
    }

    /// <summary>The rate as a percentage between 0 and 100.</summary>
    public decimal Percentage { get; }

    /// <summary>Creates a VAT rate, rejecting values outside 0–100.</summary>
    public static VatRate Of(decimal percentage)
    {
        if (percentage is < 0m or > 100m)
        {
            throw new BusinessRuleViolationException(
                "pricing.vat-rate-out-of-range",
                "VAT rate must be between 0 and 100."
            );
        }

        return new VatRate(percentage);
    }
}
