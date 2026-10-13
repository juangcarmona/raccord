using Raccord.SharedKernel.Common;

namespace Raccord.SharedKernel.Pricing;

/// <summary>Taxable base, VAT breakdown and total of a set of priced lines.</summary>
public sealed record InvoiceTotals
{
    private InvoiceTotals(IReadOnlyList<VatGroup> vatBreakdown, Money taxableBase, Money vatAmount)
    {
        VatBreakdown = vatBreakdown;
        TaxableBase = taxableBase;
        VatAmount = vatAmount;
        Total = taxableBase + vatAmount;
    }

    /// <summary>One group per VAT regime, exemption cause and rate, ordered from the highest rate.</summary>
    public IReadOnlyList<VatGroup> VatBreakdown { get; }

    /// <summary>Sum of all taxable bases.</summary>
    public Money TaxableBase { get; }

    /// <summary>Sum of the VAT amounts of all groups.</summary>
    public Money VatAmount { get; }

    /// <summary>Taxable base plus VAT.</summary>
    public Money Total { get; }

    /// <summary>
    /// Calculates totals. VAT is computed per rate on the summed net amounts and rounded once per rate,
    /// so the total never depends on how lines are split.
    /// </summary>
    public static InvoiceTotals Calculate(IEnumerable<LinePricing> lines, Currency currency)
    {
        ArgumentNullException.ThrowIfNull(lines);
        ArgumentNullException.ThrowIfNull(currency);

        var groups = lines
            .GroupBy(line => new
            {
                line.VatRate,
                line.VatRegime,
                line.ExemptionCause,
            })
            .OrderByDescending(group => group.Key.VatRate.Percentage)
            .Select(group =>
            {
                var taxableBase = group.Aggregate(
                    Money.Zero(currency),
                    (sum, line) => sum + line.NetAmount
                );
                var vat = Money.Of(
                    taxableBase.Amount * group.Key.VatRate.Percentage / 100m,
                    currency
                );
                return new VatGroup(
                    group.Key.VatRate,
                    group.Key.VatRegime,
                    group.Key.ExemptionCause,
                    taxableBase,
                    vat
                );
            })
            .ToList();

        var totalBase = groups.Aggregate(
            Money.Zero(currency),
            (sum, group) => sum + group.TaxableBase
        );
        var totalVat = groups.Aggregate(
            Money.Zero(currency),
            (sum, group) => sum + group.VatAmount
        );
        return new InvoiceTotals(groups, totalBase, totalVat);
    }
}
