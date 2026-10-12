using Raccord.SharedKernel.Common;

namespace Raccord.SharedKernel.Pricing;

/// <summary>The priced quantity of one line: what is charged and at which VAT rate.</summary>
public sealed record LinePricing
{
    /// <summary>Maximum decimals accepted for unit prices, which may be finer than invoice amounts.</summary>
    public const int UnitPriceDecimals = 4;

    private LinePricing(
        decimal quantity,
        decimal unitPrice,
        decimal discountPercent,
        VatRate vatRate,
        Currency currency
    )
    {
        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountPercent = discountPercent;
        VatRate = vatRate;
        Currency = currency;
        NetAmount = Money.Of(quantity * unitPrice * (1m - (discountPercent / 100m)), currency);
    }

    /// <summary>Units, days or hours charged.</summary>
    public decimal Quantity { get; }

    /// <summary>Net price per unit, before discount and VAT, with up to four decimals.</summary>
    public decimal UnitPrice { get; }

    /// <summary>Currency of the unit price and of the computed amounts.</summary>
    public Currency Currency { get; }

    /// <summary>Line discount between 0 and 100 percent.</summary>
    public decimal DiscountPercent { get; }

    /// <summary>VAT rate applied to the net amount.</summary>
    public VatRate VatRate { get; }

    /// <summary>Quantity × unit price × (1 − discount), rounded once to the currency's minor units.</summary>
    public Money NetAmount { get; }

    /// <summary>Creates line pricing in euros.</summary>
    public static LinePricing Create(
        decimal quantity,
        decimal unitPrice,
        decimal discountPercent,
        VatRate vatRate
    )
    {
        if (quantity <= 0m)
        {
            throw new BusinessRuleViolationException(
                "pricing.quantity-not-positive",
                "Quantity must be greater than zero."
            );
        }

        if (unitPrice < 0m)
        {
            throw new BusinessRuleViolationException(
                "pricing.unit-price-negative",
                "Unit price cannot be negative."
            );
        }

        if (decimal.Round(unitPrice, UnitPriceDecimals) != unitPrice)
        {
            throw new BusinessRuleViolationException(
                "pricing.unit-price-precision",
                "Unit price cannot have more than four decimals."
            );
        }

        if (discountPercent is < 0m or > 100m)
        {
            throw new BusinessRuleViolationException(
                "pricing.discount-out-of-range",
                "Discount must be between 0 and 100 percent."
            );
        }

        return new LinePricing(quantity, unitPrice, discountPercent, vatRate, Currency.Eur);
    }
}
