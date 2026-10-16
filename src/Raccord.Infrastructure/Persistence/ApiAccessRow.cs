namespace Raccord.Infrastructure.Persistence;

internal sealed class ApiAccessRow
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TokenHash { get; set; } = string.Empty;
    public string Scopes { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}
