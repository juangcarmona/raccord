using Raccord.Domain.Billing;

namespace Raccord.Application.Invoicing;

/// <summary>A stored issued invoice and the hash of its representation.</summary>
/// <param name="Invoice">The restored invoice.</param>
/// <param name="ContentHash">SHA-256 of the canonical representation.</param>
public sealed record IssuedInvoiceRecord(IssuedInvoice Invoice, string ContentHash);
