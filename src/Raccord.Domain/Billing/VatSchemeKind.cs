namespace Raccord.Domain.Billing;

/// <summary>How a VAT scheme resolves its calculated percentage.</summary>
public enum VatSchemeKind
{
    /// <summary>Uses an explicit rate for the selected VAT class.</summary>
    PerClass,

    /// <summary>Uses one configured rate for any selected VAT class.</summary>
    FixedRate,

    /// <summary>Calculates no VAT percentage; invoice disclosure remains a separate capability.</summary>
    ReverseCharge,
}
