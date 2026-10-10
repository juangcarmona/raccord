using Raccord.Domain.Common;
using Raccord.Domain.Pricing;

namespace Raccord.Domain.Tests.Pricing;

public sealed class InvoiceTotalsTests
{
    private static readonly VatRate General = VatRate.Of(21m);
    private static readonly VatRate Reduced = VatRate.Of(10m);

    [Fact]
    public void Vat_is_calculated_per_rate_on_the_summed_taxable_base()
    {
        // Rounding each line would give 3 × 0.02 = 0.06; the base-level method gives round(0.045) = 0.05.
        var lines = Enumerable.Range(0, 3).Select(_ => LinePricing.Create(1m, 0.15m, 0m, Reduced));

        var totals = InvoiceTotals.Calculate(lines, Currency.Eur);

        var group = Assert.Single(totals.VatBreakdown);
        Assert.Equal(0.45m, group.TaxableBase.Amount);
        Assert.Equal(0.05m, group.VatAmount.Amount);
    }

    [Fact]
    public void Lines_are_grouped_by_rate_and_ordered_from_highest_rate()
    {
        var lines = new[]
        {
            LinePricing.Create(2m, 100m, 0m, Reduced),
            LinePricing.Create(1m, 1200m, 0m, General),
            LinePricing.Create(4m, 75.50m, 10m, General),
        };

        var totals = InvoiceTotals.Calculate(lines, Currency.Eur);

        Assert.Collection(
            totals.VatBreakdown,
            general =>
            {
                Assert.Equal(General, general.Rate);
                Assert.Equal(1471.80m, general.TaxableBase.Amount);
                Assert.Equal(309.08m, general.VatAmount.Amount);
            },
            reduced =>
            {
                Assert.Equal(Reduced, reduced.Rate);
                Assert.Equal(200.00m, reduced.TaxableBase.Amount);
                Assert.Equal(20.00m, reduced.VatAmount.Amount);
            });
        Assert.Equal(1671.80m, totals.TaxableBase.Amount);
        Assert.Equal(329.08m, totals.VatAmount.Amount);
        Assert.Equal(2000.88m, totals.Total.Amount);
    }

    [Fact]
    public void Zero_rated_lines_still_appear_in_the_breakdown()
    {
        var totals = InvoiceTotals.Calculate([LinePricing.Create(1m, 50m, 0m, VatRate.Of(0m))], Currency.Eur);

        var group = Assert.Single(totals.VatBreakdown);
        Assert.Equal(0m, group.VatAmount.Amount);
        Assert.Equal(50m, totals.Total.Amount);
    }

    [Fact]
    public void No_lines_give_zero_totals()
    {
        var totals = InvoiceTotals.Calculate([], Currency.Eur);

        Assert.Empty(totals.VatBreakdown);
        Assert.Equal(Money.Zero(Currency.Eur), totals.Total);
    }
}
