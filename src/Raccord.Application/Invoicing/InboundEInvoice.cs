namespace Raccord.Application.Invoicing;

/// <summary>An inbound structured invoice persisted by the application layer.</summary>
public sealed class InboundEInvoice
{
    /// <summary>The stable invoice identifier.</summary>
    public Guid Id { get; init; }

    /// <summary>The supplier's invoice number.</summary>
    public required string InvoiceNumber { get; init; }

    /// <summary>The supplier's legal name.</summary>
    public required string SupplierName { get; init; }

    /// <summary>The supplier's tax identifier.</summary>
    public required string SupplierTaxId { get; init; }

    /// <summary>The date stated on the supplier invoice.</summary>
    public DateOnly IssueDate { get; init; }

    /// <summary>The payable amount stated on the supplier invoice.</summary>
    public decimal PayableAmount { get; init; }

    /// <summary>The invoice currency.</summary>
    public required string Currency { get; init; }

    /// <summary>The instant at which Raccord received the document.</summary>
    public DateTimeOffset ReceivedAt { get; init; }

    /// <summary>The reporting deadline calculated from receipt.</summary>
    public DateOnly StatusDueOn { get; init; }

    /// <summary>The current business status.</summary>
    public required string Status { get; set; }

    /// <summary>The reason supplied when the invoice is rejected.</summary>
    public string? Reason { get; set; }

    /// <summary>The date on which the invoice was fully paid.</summary>
    public DateOnly? PaidOn { get; set; }

    /// <summary>The SHA-256 hash of the received XML.</summary>
    public required string ContentHash { get; init; }

    /// <summary>The original structured invoice XML.</summary>
    public required string Xml { get; init; }
}
