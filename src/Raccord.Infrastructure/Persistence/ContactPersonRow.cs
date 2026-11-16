namespace Raccord.Infrastructure.Persistence;

/// <summary>Persistence representation of a person owned by a Contacts record.</summary>
internal sealed class ContactPersonRow
{
    public Guid Id { get; set; }

    public Guid ContactId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool IsDefault { get; set; }

    public bool IsAdministrative { get; set; }
}
