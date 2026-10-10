namespace Raccord.Domain.Billing;

/// <summary>Lifecycle of an invoice draft.</summary>
public enum DraftStatus
{
    /// <summary>Editable and not yet numbered.</summary>
    Open = 1,

    /// <summary>Finalized into an issued invoice; read-only.</summary>
    Issued = 2,
}
