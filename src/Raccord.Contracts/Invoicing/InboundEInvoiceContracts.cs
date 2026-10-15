namespace Raccord.Contracts.Invoicing;

/// <summary>Receives a structured invoice from the configured exchange boundary.</summary>
public sealed record ReceiveInboundEInvoiceRequest(string Xml);

/// <summary>Changes the business status of an inbound e-invoice.</summary>
public sealed record UpdateInboundEInvoiceStatusRequest(
    string Status,
    string? Reason = null,
    DateOnly? PaidOn = null
);

/// <summary>An inbound structured invoice and its current reporting status.</summary>
public sealed record InboundEInvoiceResponse(
    Guid Id,
    string InvoiceNumber,
    string SupplierName,
    string SupplierTaxId,
    DateOnly IssueDate,
    decimal PayableAmount,
    string Currency,
    DateTimeOffset ReceivedAt,
    DateOnly StatusDueOn,
    string Status,
    string? Reason,
    DateOnly? PaidOn,
    string ContentHash
);
