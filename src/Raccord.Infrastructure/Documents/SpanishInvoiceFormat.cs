using System.Globalization;

namespace Raccord.Infrastructure.Documents;

/// <summary>
/// Spanish (es-ES) presentation of numbers and dates for invoice documents. Implemented without culture data
/// so the output is identical on every operating system and ICU version.
/// </summary>
internal static class SpanishInvoiceFormat
{
    public static string Money(decimal amount, string currencyCode) =>
        Grouped(amount, "F2") + " " + (string.Equals(currencyCode, "EUR", StringComparison.Ordinal) ? "€" : currencyCode);

    public static string Quantity(decimal value) => Grouped(value, "0.####");

    public static string UnitPrice(decimal value) => Grouped(value, "0.00##");

    public static string Percent(decimal value) => Grouped(value, "0.##") + " %";

    public static string Date(DateOnly date) => date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

    private static string Grouped(decimal value, string format)
    {
        var invariant = Math.Abs(value).ToString(format, CultureInfo.InvariantCulture);
        var separator = invariant.IndexOf('.', StringComparison.Ordinal);
        var integer = separator < 0 ? invariant : invariant[..separator];
        var fraction = separator < 0 ? string.Empty : "," + invariant[(separator + 1)..];

        var grouped = new System.Text.StringBuilder();
        for (var i = 0; i < integer.Length; i++)
        {
            if (i > 0 && (integer.Length - i) % 3 == 0)
            {
                grouped.Append('.');
            }

            grouped.Append(integer[i]);
        }

        return (value < 0 ? "-" : string.Empty) + grouped + fraction;
    }
}
