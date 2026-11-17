namespace Raccord.Infrastructure.Persistence;

/// <summary>A reusable Contacts tag in this deployment database.</summary>
internal sealed class ContactTagRow
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string NameKey { get; set; } = string.Empty;

    public List<ContactTagAssignmentRow> Assignments { get; set; } = [];
}
