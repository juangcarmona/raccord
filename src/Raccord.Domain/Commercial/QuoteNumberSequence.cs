using System.Globalization;
using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Commercial;

/// <summary>Gapless numbering for quotes in one series and calendar year.</summary>
public sealed class QuoteNumberSequence
{
    private QuoteNumberSequence()
    {
        SeriesCode = string.Empty;
    }

    private QuoteNumberSequence(string seriesCode, int year)
    {
        SeriesCode = seriesCode;
        Year = year;
    }

    /// <summary>Series code.</summary>
    public string SeriesCode { get; private set; }

    /// <summary>Calendar year of this sequence.</summary>
    public int Year { get; private set; }

    /// <summary>Last allocated sequence number.</summary>
    public int LastNumber { get; private set; }

    /// <summary>Starts an empty quote sequence.</summary>
    public static QuoteNumberSequence Start(string seriesCode, int year)
    {
        if (
            string.IsNullOrWhiteSpace(seriesCode)
            || seriesCode.Length > 10
            || seriesCode.Any(character =>
                (character < 'A' || character > 'Z') && !char.IsAsciiDigit(character)
            )
        )
            throw new BusinessRuleViolationException(
                "quotes.series-invalid",
                "Quote series must be 1 to 10 uppercase letters or digits."
            );
        return new QuoteNumberSequence(seriesCode, year);
    }

    /// <summary>Allocates the next quote number.</summary>
    public QuoteNumber Allocate()
    {
        LastNumber++;
        return new(
            SeriesCode,
            Year,
            LastNumber,
            string.Create(CultureInfo.InvariantCulture, $"{SeriesCode}{Year}-{LastNumber:D5}")
        );
    }
}
