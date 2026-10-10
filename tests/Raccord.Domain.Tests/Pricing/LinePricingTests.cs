using Raccord.Domain.Common;
using Raccord.Domain.Pricing;

namespace Raccord.Domain.Tests.Pricing;

public sealed class LinePricingTests
{
    [Fact]
    public void Net_amount_applies_discount_and_rounds_once_at_the_end()
    {
        var pricing = LinePricing.Create(
            quantity: 3m,
            unitPrice: 33.333m,
            discountPercent: 10m,
            VatRate.Of(21m)
        );

        Assert.Equal(Money.Of(90.00m, Currency.Eur), pricing.NetAmount);
    }

    [Fact]
    public void Fractional_quantities_such_as_days_or_hours_are_supported()
    {
        var pricing = LinePricing.Create(
            quantity: 1.5m,
            unitPrice: 450m,
            discountPercent: 0m,
            VatRate.Of(21m)
        );

        Assert.Equal(675.00m, pricing.NetAmount.Amount);
    }

    [Theory]
    [InlineData(0, 10, 0, "pricing.quantity-not-positive")]
    [InlineData(-1, 10, 0, "pricing.quantity-not-positive")]
    [InlineData(1, -0.01, 0, "pricing.unit-price-negative")]
    [InlineData(1, 10, -5, "pricing.discount-out-of-range")]
    [InlineData(1, 10, 100.01, "pricing.discount-out-of-range")]
    public void Invalid_inputs_are_rejected_with_a_stable_rule_code(
        double quantity,
        double unitPrice,
        double discount,
        string expectedCode
    )
    {
        var error = Assert.Throws<BusinessRuleViolationException>(() =>
            LinePricing.Create(
                (decimal)quantity,
                (decimal)unitPrice,
                (decimal)discount,
                VatRate.Of(21m)
            )
        );

        Assert.Equal(expectedCode, error.Code);
    }

    [Fact]
    public void Unit_prices_keep_up_to_four_decimals()
    {
        var pricing = LinePricing.Create(
            quantity: 1000m,
            unitPrice: 0.1234m,
            discountPercent: 0m,
            VatRate.Of(21m)
        );

        Assert.Equal(0.1234m, pricing.UnitPrice);
        Assert.Equal(123.40m, pricing.NetAmount.Amount);
    }

    [Fact]
    public void Unit_prices_with_more_than_four_decimals_are_rejected()
    {
        var error = Assert.Throws<BusinessRuleViolationException>(() =>
            LinePricing.Create(1m, 0.12345m, 0m, VatRate.Of(21m))
        );

        Assert.Equal("pricing.unit-price-precision", error.Code);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(100.01)]
    public void Vat_rates_must_be_between_zero_and_one_hundred(double rate)
    {
        var error = Assert.Throws<BusinessRuleViolationException>(() => VatRate.Of((decimal)rate));

        Assert.Equal("pricing.vat-rate-out-of-range", error.Code);
    }
}
