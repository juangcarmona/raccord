using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ContactFolderConfiguration : IEntityTypeConfiguration<ContactFolderRow>
{
    public void Configure(EntityTypeBuilder<ContactFolderRow> builder)
    {
        builder.ToTable("contact_folders", "contacts");
        builder.HasKey(folder => folder.Id);
        builder.Property(folder => folder.Name).IsRequired();
        builder
            .HasOne<ContactFolderRow>()
            .WithMany()
            .HasForeignKey(folder => folder.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(folder => new { folder.ParentId, folder.Position });
    }
}
