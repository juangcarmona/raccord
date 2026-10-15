namespace Raccord.Domain.Billing;

/// <summary>Where an issued invoice came from.</summary>
/// <param name="DraftId">The finalized draft.</param>
/// <param name="ProjectId">The billed project, when this is a project invoice.</param>
/// <param name="CustomerId">The billed customer.</param>
/// <param name="OriginalInvoiceId">The invoice being corrected.</param>
/// <param name="RectificationKind">The correction method.</param>
/// <param name="RectificationReason">The stated reason.</param>
public sealed record IssuedInvoiceSource(
    Guid DraftId,
    Guid? ProjectId,
    Guid CustomerId,
    Guid? OriginalInvoiceId = null,
    RectificationKind? RectificationKind = null,
    string? RectificationReason = null
);
