namespace Raccord.Infrastructure.Persistence;

internal sealed class ContactPersonRow
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsDefault { get; set; }
}
