namespace Raccord.Domain.Billing;

/// <summary>How a new invoice corrects its original.</summary>
public enum RectificationKind
{
    /// <summary>Only the correcting difference.</summary>
    Differences,

    /// <summary>The corrected complete invoice.</summary>
    Substitution,
}
