namespace Raccord.Domain.Common;

/// <summary>A monetary amount rounded to the minor units of its currency.</summary>
public readonly record struct Money
{
    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>The rounded amount.</summary>
    public decimal Amount { get; }

    /// <summary>The currency of the amount.</summary>
    public Currency Currency { get; }

    /// <summary>Creates money, rounding to the currency's minor units with midpoint away from zero.</summary>
    public static Money Of(decimal amount, Currency currency)
    {
        ArgumentNullException.ThrowIfNull(currency);
        return new Money(
            Math.Round(amount, currency.MinorUnits, MidpointRounding.AwayFromZero),
            currency
        );
    }

    /// <summary>A zero amount in the given currency.</summary>
    public static Money Zero(Currency currency) => Of(0m, currency);

    /// <summary>Adds two amounts of the same currency.</summary>
    public static Money operator +(Money left, Money right)
    {
        EnsureSameCurrency(left, right);
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    /// <summary>Adds two amounts of the same currency.</summary>
    public static Money Add(Money left, Money right) => left + right;

    private static void EnsureSameCurrency(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new BusinessRuleViolationException(
                "money.currency-mismatch",
                "Amounts in different currencies cannot be combined."
            );
        }
    }
}
