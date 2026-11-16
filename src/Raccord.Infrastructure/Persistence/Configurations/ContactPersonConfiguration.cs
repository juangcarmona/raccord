using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Infrastructure.Persistence;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ContactPersonConfiguration : IEntityTypeConfiguration<ContactPersonRow>
{
    public void Configure(EntityTypeBuilder<ContactPersonRow> builder)
    {
        builder.ToTable("contact_persons", "contacts");
        builder.HasKey(person => person.Id);
        builder.Property(person => person.Name).HasMaxLength(200).IsRequired();
        builder.Property(person => person.Email).HasMaxLength(320);
        builder.Property(person => person.Phone).HasMaxLength(64);
        builder
            .HasOne<CustomerRow>()
            .WithMany()
            .HasForeignKey(person => person.ContactId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasIndex(person => person.ContactId, "ix_contact_persons_contact_default")
            .IsUnique()
            .HasFilter("is_default = TRUE")
            .HasDatabaseName("ix_contact_persons_contact_default");
        builder
            .HasIndex(person => person.ContactId, "ix_contact_persons_contact_administrative")
            .IsUnique()
            .HasFilter("is_administrative = TRUE")
            .HasDatabaseName("ix_contact_persons_contact_administrative");
        builder
            .HasIndex(person => new { person.ContactId, person.Name })
            .HasDatabaseName("ix_contact_persons_contact_name");
    }
}
