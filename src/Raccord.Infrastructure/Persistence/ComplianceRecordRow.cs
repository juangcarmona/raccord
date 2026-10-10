namespace Raccord.Infrastructure.Persistence;

/// <summary>
/// A pending compliance obligation created in the same transaction as an issued invoice.
/// A future tax-authority adapter claims pending rows, submits them and records the outcome.
/// </summary>
internal sealed class ComplianceRecordRow
{
    public Guid Id { get; init; }

    public Guid InvoiceId { get; init; }

    public required string InvoiceNumber { get; init; }

    public required string ContentHash { get; init; }

    public required string Kind { get; init; }

    public required string Status { get; set; }

    public DateTimeOffset CreatedAt { get; init; }

    public int Attempts { get; set; }

    public DateTimeOffset? LastAttemptAt { get; set; }

    public string? LastError { get; set; }
}
