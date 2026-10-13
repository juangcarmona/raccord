namespace Raccord.Modules.Compliance;

/// <summary>A pending compliance obligation created with the issued invoice.</summary>
public sealed class ComplianceRecordRow
{
    public Guid Id { get; init; }
    public Guid InvoiceId { get; init; }
    public required string InvoiceNumber { get; init; }
    public required string ContentHash { get; init; }
    public string? PreviousHash { get; init; }
    public required string RecordHash { get; init; }
    public required string Kind { get; init; }
    public required string Status { get; set; }
    public DateTimeOffset CreatedAt { get; init; }
    public int Attempts { get; set; }
    public DateTimeOffset? LastAttemptAt { get; set; }
    public string? LastError { get; set; }
}
