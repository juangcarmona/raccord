namespace Raccord.Infrastructure.Persistence;

internal sealed class CommunicationRow
{
    public Guid Id { get; set; }
    public string Recipient { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? DocumentKey { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? FailureReason { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? DeliveredAt { get; set; }
}
