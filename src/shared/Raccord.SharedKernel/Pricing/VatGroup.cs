using Raccord.SharedKernel.Common;

namespace Raccord.SharedKernel.Pricing;

/// <summary>The taxable base and VAT amount for one VAT regime and rate.</summary>
/// <param name="Rate">The VAT rate of the group.</param>
/// <param name="VatRegime">Tax authority VAT regime key.</param>
/// <param name="ExemptionCause">VAT exemption cause, when applicable.</param>
/// <param name="TaxableBase">Sum of the net amounts of the lines at this rate.</param>
/// <param name="VatAmount">VAT on the taxable base, rounded once.</param>
public sealed record VatGroup(
    VatRate Rate,
    string VatRegime,
    string? ExemptionCause,
    Money TaxableBase,
    Money VatAmount
);
