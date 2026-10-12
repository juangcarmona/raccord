using Raccord.SharedKernel.Common;

namespace Raccord.SharedKernel.Pricing;

/// <summary>The taxable base and VAT amount for one VAT rate.</summary>
/// <param name="Rate">The VAT rate of the group.</param>
/// <param name="TaxableBase">Sum of the net amounts of the lines at this rate.</param>
/// <param name="VatAmount">VAT on the taxable base, rounded once.</param>
public sealed record VatGroup(VatRate Rate, Money TaxableBase, Money VatAmount);
