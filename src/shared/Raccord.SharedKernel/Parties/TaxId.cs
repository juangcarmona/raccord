namespace Raccord.SharedKernel.Parties;

/// <summary>Normalization of tax identification numbers.</summary>
public static class TaxId
{
    /// <summary>Uppercases and removes spaces, dots and hyphens; returns null when nothing remains.</summary>
    public static string? Normalize(string? raw)
    {
        if (raw is null)
        {
            return null;
        }

        var normalized = new string([
            .. raw.Where(char.IsAsciiLetterOrDigit).Select(char.ToUpperInvariant),
        ]);
        return normalized.Length == 0 ? null : normalized;
    }
}
