using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Tests.Pricing;

public sealed class InvoiceTotalsTests
{
    private static readonly VatRate General = VatRate.Of(21m);
    private static readonly VatRate Reduced = VatRate.Of(10m);

    [Fact]
    public void Vat_Should_be_calculated_per_rate_on_the_summed_taxable_base()
    {
        // Rounding each line would give 3 × 0.02 = 0.06; the base-level method gives round(0.045) = 0.05.
        var lines = Enumerable.Range(0, 3).Select(_ => LinePricing.Create(1m, 0.15m, 0m, Reduced));

        var totals = InvoiceTotals.Calculate(lines, Currency.Eur);

        var group = totals.VatBreakdown.ShouldHaveSingleItem();
        group.TaxableBase.Amount.ShouldBe(0.45m);
        group.VatAmount.Amount.ShouldBe(0.05m);
    }

    [Fact]
    public void Lines_Should_be_grouped_by_rate_and_ordered_from_highest_rate()
    {
        var lines = new[]
        {
            LinePricing.Create(2m, 100m, 0m, Reduced),
            LinePricing.Create(1m, 1200m, 0m, General),
            LinePricing.Create(4m, 75.50m, 10m, General),
        };

        var totals = InvoiceTotals.Calculate(lines, Currency.Eur);

        var breakdown = totals.VatBreakdown.ToArray();
        breakdown.Length.ShouldBe(2);
        breakdown[0].Rate.ShouldBe(General);
        breakdown[0].TaxableBase.Amount.ShouldBe(1471.80m);
        breakdown[0].VatAmount.Amount.ShouldBe(309.08m);
        breakdown[1].Rate.ShouldBe(Reduced);
        breakdown[1].TaxableBase.Amount.ShouldBe(200.00m);
        breakdown[1].VatAmount.Amount.ShouldBe(20.00m);
        totals.TaxableBase.Amount.ShouldBe(1671.80m);
        totals.VatAmount.Amount.ShouldBe(329.08m);
        totals.Total.Amount.ShouldBe(2000.88m);
    }

    [Fact]
    public void Zero_rated_lines_Should_still_appear_in_the_breakdown()
    {
        var totals = InvoiceTotals.Calculate(
            [LinePricing.Create(1m, 50m, 0m, VatRate.Of(0m))],
            Currency.Eur
        );

        var group = totals.VatBreakdown.ShouldHaveSingleItem();
        group.VatAmount.Amount.ShouldBe(0m);
        totals.Total.Amount.ShouldBe(50m);
    }

    [Fact]
    public void No_lines_Should_give_zero_totals()
    {
        var totals = InvoiceTotals.Calculate([], Currency.Eur);

        totals.VatBreakdown.ShouldBeEmpty();
        totals.Total.ShouldBe(Money.Zero(Currency.Eur));
    }
}
