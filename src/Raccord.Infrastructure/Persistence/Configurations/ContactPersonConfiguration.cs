using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ContactPersonConfiguration : IEntityTypeConfiguration<ContactPersonRow>
{
    public void Configure(EntityTypeBuilder<ContactPersonRow> builder)
    {
        builder.ToTable("contact_persons");
        builder.HasKey(person => person.Id);
        builder.Property(person => person.Name).HasMaxLength(200).IsRequired();
        builder.Property(person => person.Role).HasMaxLength(100).IsRequired();
        builder.Property(person => person.Email).HasMaxLength(320);
        builder.Property(person => person.Phone).HasMaxLength(50);
        builder
            .HasIndex(person => new { person.CustomerId, person.IsDefault })
            .IsUnique()
            .HasFilter("is_default");
    }
}
