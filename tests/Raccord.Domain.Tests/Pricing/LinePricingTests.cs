using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Tests.Pricing;

public sealed class LinePricingTests
{
    [Fact]
    public void Net_amount_Should_apply_discount_and_round_once_at_the_end()
    {
        var pricing = LinePricing.Create(
            quantity: 3m,
            unitPrice: 33.333m,
            discountPercent: 10m,
            VatRate.Of(21m)
        );

        pricing.NetAmount.ShouldBe(Money.Of(90.00m, Currency.Eur));
    }

    [Fact]
    public void Fractional_quantities_such_as_days_or_hours_Should_be_supported()
    {
        var pricing = LinePricing.Create(
            quantity: 1.5m,
            unitPrice: 450m,
            discountPercent: 0m,
            VatRate.Of(21m)
        );

        pricing.NetAmount.Amount.ShouldBe(675.00m);
    }

    [Theory]
    [InlineData(0, 10, 0, "pricing.quantity-not-positive")]
    [InlineData(-1, 10, 0, "pricing.quantity-not-positive")]
    [InlineData(1, -0.01, 0, "pricing.unit-price-negative")]
    [InlineData(1, 10, -5, "pricing.discount-out-of-range")]
    [InlineData(1, 10, 100.01, "pricing.discount-out-of-range")]
    public void Invalid_inputs_Should_be_rejected_with_a_stable_rule_code(
        double quantity,
        double unitPrice,
        double discount,
        string expectedCode
    )
    {
        var error = Should.Throw<BusinessRuleViolationException>(() =>
            LinePricing.Create(
                (decimal)quantity,
                (decimal)unitPrice,
                (decimal)discount,
                VatRate.Of(21m)
            )
        );

        error.Code.ShouldBe(expectedCode);
    }

    [Fact]
    public void Unit_prices_Should_keep_up_to_four_decimals()
    {
        var pricing = LinePricing.Create(
            quantity: 1000m,
            unitPrice: 0.1234m,
            discountPercent: 0m,
            VatRate.Of(21m)
        );

        pricing.UnitPrice.ShouldBe(0.1234m);
        pricing.NetAmount.Amount.ShouldBe(123.40m);
    }

    [Fact]
    public void Unit_prices_with_more_than_four_decimals_Should_be_rejected()
    {
        var error = Should.Throw<BusinessRuleViolationException>(() =>
            LinePricing.Create(1m, 0.12345m, 0m, VatRate.Of(21m))
        );

        error.Code.ShouldBe("pricing.unit-price-precision");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(100.01)]
    public void Vat_rates_Should_be_between_zero_and_one_hundred(double rate)
    {
        var error = Should.Throw<BusinessRuleViolationException>(() => VatRate.Of((decimal)rate));

        error.Code.ShouldBe("pricing.vat-rate-out-of-range");
    }
}
