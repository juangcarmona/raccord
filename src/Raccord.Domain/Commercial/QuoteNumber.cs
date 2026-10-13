namespace Raccord.Domain.Commercial;

/// <summary>An allocated quote number.</summary>
public sealed record QuoteNumber(string SeriesCode, int Year, int Sequence, string Value);
