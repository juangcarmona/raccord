namespace Raccord.Domain.Billing;

/// <summary>An allocated invoice number.</summary>
/// <param name="SeriesCode">The series the number belongs to.</param>
/// <param name="Year">The calendar year of the sequence.</param>
/// <param name="Sequence">The correlative position within the series and year.</param>
/// <param name="Value">The printed number, for example <c>F2026-00001</c>.</param>
public sealed record InvoiceNumber(string SeriesCode, int Year, int Sequence, string Value);
