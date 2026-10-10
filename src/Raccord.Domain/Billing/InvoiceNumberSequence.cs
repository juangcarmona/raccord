using System.Globalization;
using Raccord.Domain.Common;

namespace Raccord.Domain.Billing;

/// <summary>
/// The gapless, correlative numbering of one invoice series in one calendar year.
/// Numbers are allocated only when an invoice is issued, inside the issuing transaction.
/// </summary>
public sealed class InvoiceNumberSequence
{
    private const int MaxSeriesCodeLength = 10;

    private InvoiceNumberSequence()
    {
        SeriesCode = string.Empty;
    }

    private InvoiceNumberSequence(string seriesCode, int year)
    {
        SeriesCode = seriesCode;
        Year = year;
    }

    /// <summary>Series code, for example <c>F</c>.</summary>
    public string SeriesCode { get; private set; }

    /// <summary>Calendar year numbered by this sequence.</summary>
    public int Year { get; private set; }

    /// <summary>The last allocated position; zero before the first invoice.</summary>
    public int LastNumber { get; private set; }

    /// <summary>Issue date of the last allocated number.</summary>
    public DateOnly? LastIssueDate { get; private set; }

    /// <summary>Starts an empty sequence.</summary>
    public static InvoiceNumberSequence Start(string seriesCode, int year)
    {
        EnsureValidSeriesCode(seriesCode);
        return new InvoiceNumberSequence(seriesCode, year);
    }

    /// <summary>Validates a series code: 1–10 characters, uppercase letters and digits.</summary>
    public static void EnsureValidSeriesCode(string seriesCode)
    {
        if (
            string.IsNullOrEmpty(seriesCode)
            || seriesCode.Length > MaxSeriesCodeLength
            || !seriesCode.All(c => char.IsAsciiDigit(c) || char.IsAsciiLetterUpper(c))
        )
        {
            throw new BusinessRuleViolationException(
                "numbering.series-code-invalid",
                "Series codes have 1 to 10 uppercase letters or digits."
            );
        }
    }

    /// <summary>Whether an invoice with this issue date may be numbered next.</summary>
    public bool Accepts(DateOnly issueDate) =>
        issueDate.Year == Year && (LastIssueDate is null || issueDate >= LastIssueDate);

    /// <summary>Allocates the next number for an invoice issued on the given date.</summary>
    public InvoiceNumber Allocate(DateOnly issueDate)
    {
        if (issueDate.Year != Year)
        {
            throw new BusinessRuleViolationException(
                "numbering.year-mismatch",
                $"Sequence {SeriesCode}/{Year} cannot number an invoice dated {issueDate:yyyy-MM-dd}."
            );
        }

        if (LastIssueDate is { } last && issueDate < last)
        {
            throw new BusinessRuleViolationException(
                "numbering.issue-date-before-previous",
                $"Issue date {issueDate:yyyy-MM-dd} is earlier than the last invoice of series {SeriesCode} ({last:yyyy-MM-dd})."
            );
        }

        LastNumber++;
        LastIssueDate = issueDate;
        var value = string.Create(
            CultureInfo.InvariantCulture,
            $"{SeriesCode}{Year}-{LastNumber:D5}"
        );
        return new InvoiceNumber(SeriesCode, Year, LastNumber, value);
    }
}
