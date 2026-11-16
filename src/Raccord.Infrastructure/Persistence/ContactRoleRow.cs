using Raccord.Modules.Contacts.Contracts;

namespace Raccord.Infrastructure.Persistence;

/// <summary>One supported directory role for a contact.</summary>
internal sealed class ContactRoleRow
{
    public Guid ContactId { get; set; }

    public ContactRole Role { get; set; }

    public CustomerRow Contact { get; set; } = null!;
}
