using System.Globalization;

namespace Raccord.Infrastructure.Documents;

internal static class EnglishInvoiceFormat
{
    public static string Money(decimal amount, string currencyCode) =>
        amount.ToString("N2", CultureInfo.InvariantCulture)
        + " "
        + (string.Equals(currencyCode, "EUR", StringComparison.Ordinal) ? "€" : currencyCode);

    public static string Quantity(decimal value) =>
        value.ToString("0.####", CultureInfo.InvariantCulture);

    public static string UnitPrice(decimal value) =>
        value.ToString("N2", CultureInfo.InvariantCulture);

    public static string Percent(decimal value) =>
        value.ToString("N2", CultureInfo.InvariantCulture) + " %";

    public static string Date(DateOnly date) =>
        date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
}
