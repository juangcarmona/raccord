using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ContactTagConfiguration : IEntityTypeConfiguration<ContactTagRow>
{
    public void Configure(EntityTypeBuilder<ContactTagRow> builder)
    {
        builder.ToTable("contact_tags", "contacts");
        builder.HasKey(tag => tag.Id);
        builder.Property(tag => tag.Name).IsRequired();
        builder.Property(tag => tag.NameKey).HasMaxLength(64).IsRequired();
        builder.HasIndex(tag => tag.NameKey).IsUnique();
    }
}
