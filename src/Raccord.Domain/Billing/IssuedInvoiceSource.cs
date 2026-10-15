namespace Raccord.Domain.Billing;

/// <summary>Where an issued invoice came from.</summary>
/// <param name="DraftId">The finalized draft.</param>
/// <param name="ProjectId">The billed project.</param>
/// <param name="CustomerId">The billed customer.</param>
public sealed record IssuedInvoiceSource(Guid DraftId, Guid? ProjectId, Guid CustomerId);
