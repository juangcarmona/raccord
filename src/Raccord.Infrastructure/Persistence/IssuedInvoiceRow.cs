namespace Raccord.Infrastructure.Persistence;

/// <summary>
/// Storage of an issued invoice: indexed columns for queries plus the canonical representation, which is the source of truth.
/// The table rejects UPDATE and DELETE at the database level.
/// </summary>
internal sealed class IssuedInvoiceRow
{
    public Guid Id { get; init; }

    public required string SeriesCode { get; init; }

    public int Year { get; init; }

    public int Sequence { get; init; }

    public required string Number { get; init; }

    public DateOnly IssueDate { get; init; }

    public DateTimeOffset IssuedAt { get; init; }

    public Guid DraftId { get; init; }

    public Guid? ProjectId { get; init; }

    public Guid CustomerId { get; init; }

    public required string RecipientName { get; init; }

    public string? RecipientTaxId { get; init; }

    public decimal Total { get; init; }

    public required string Currency { get; init; }

    public required string Representation { get; init; }

    public required string ContentHash { get; init; }
}
