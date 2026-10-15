namespace Raccord.Infrastructure.Persistence;

internal sealed class CalendarFeedRow
{
    public Guid Id { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public string Stream { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }
}
