namespace Raccord.Infrastructure.Persistence;

/// <summary>An assignment of one Contacts tag to one contact.</summary>
internal sealed class ContactTagAssignmentRow
{
    public Guid ContactId { get; set; }

    public Guid TagId { get; set; }

    public CustomerRow Contact { get; set; } = null!;

    public ContactTagRow Tag { get; set; } = null!;
}
