using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Infrastructure.Persistence;
using Raccord.Modules.Contacts.Contracts;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ContactRoleConfiguration : IEntityTypeConfiguration<ContactRoleRow>
{
    public void Configure(EntityTypeBuilder<ContactRoleRow> builder)
    {
        builder.ToTable("contact_roles", "contacts");
        builder.HasKey(role => new { role.ContactId, role.Role });
        builder.Property(role => role.Role).HasConversion<string>().HasMaxLength(32);
    }
}
