using Raccord.Domain.Common;

namespace Raccord.Domain.Tests.Common;

public sealed class MoneyTests
{
    [Theory]
    [InlineData("10.005", "10.01")]
    [InlineData("10.004", "10.00")]
    [InlineData("-10.005", "-10.01")]
    [InlineData("0.125", "0.13")]
    public void Amounts_are_rounded_to_cents_with_midpoint_away_from_zero(
        string raw,
        string expected
    )
    {
        var money = Money.Of(
            decimal.Parse(raw, System.Globalization.CultureInfo.InvariantCulture),
            Currency.Eur
        );

        Assert.Equal(
            decimal.Parse(expected, System.Globalization.CultureInfo.InvariantCulture),
            money.Amount
        );
    }
}
