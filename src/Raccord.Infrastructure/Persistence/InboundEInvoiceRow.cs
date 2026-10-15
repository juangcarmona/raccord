namespace Raccord.Infrastructure.Persistence;

internal sealed class InboundEInvoiceRow
{
    public Guid Id { get; init; }
    public required string InvoiceNumber { get; init; }
    public required string SupplierName { get; init; }
    public required string SupplierTaxId { get; init; }
    public DateOnly IssueDate { get; init; }
    public decimal PayableAmount { get; init; }
    public required string Currency { get; init; }
    public DateTimeOffset ReceivedAt { get; init; }
    public DateOnly StatusDueOn { get; init; }
    public required string Status { get; set; }
    public string? Reason { get; set; }
    public DateOnly? PaidOn { get; set; }
    public required string ContentHash { get; init; }
    public required string Xml { get; init; }
}
