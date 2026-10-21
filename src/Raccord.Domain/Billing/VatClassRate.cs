using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Billing;

/// <summary>A VAT percentage mapped to one stable VAT class identity.</summary>
public sealed class VatClassRate
{
    private VatClassRate() { }

    /// <summary>Creates a validated mapping for one VAT class.</summary>
    public VatClassRate(Guid vatClassId, decimal percentage)
    {
        if (vatClassId == Guid.Empty)
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-class-identity-invalid",
                "A VAT class mapping requires a stable class identity."
            );
        }

        VatClassId = vatClassId;
        Percentage = VatRate.Of(percentage).Percentage;
    }

    /// <summary>Stable VAT class identity.</summary>
    public Guid VatClassId { get; private set; }

    /// <summary>Percentage captured by the scheme.</summary>
    public decimal Percentage { get; private set; }
}
