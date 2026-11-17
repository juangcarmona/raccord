namespace Raccord.Infrastructure.Persistence;

/// <summary>A node in the Contacts folder tree for this deployment.</summary>
internal sealed class ContactFolderRow
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }

    public int Position { get; set; }
}
