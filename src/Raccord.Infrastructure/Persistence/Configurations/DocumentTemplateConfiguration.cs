using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class DocumentTemplateConfiguration : IEntityTypeConfiguration<DocumentTemplateRow>
{
    public void Configure(EntityTypeBuilder<DocumentTemplateRow> builder)
    {
        builder.ToTable("document_templates", "workspace");
        builder.HasKey(template => template.Id);
        builder.Property(template => template.Name).HasMaxLength(200).IsRequired();
        builder.Property(template => template.DocumentType).HasMaxLength(64).IsRequired();
        builder.Property(template => template.Language).HasMaxLength(16).IsRequired();
        builder.Property(template => template.Letterhead).HasMaxLength(2_000).IsRequired();
        builder.Property(template => template.BodyTemplate).HasMaxLength(20_000).IsRequired();
        builder.Property(template => template.Footer).HasMaxLength(2_000).IsRequired();
        builder.Property(template => template.FontFamily).HasMaxLength(100).IsRequired();
        builder.Property(template => template.AccentColor).HasMaxLength(32).IsRequired();
        builder.Property(template => template.LogoStorageKey).HasMaxLength(500);
        builder.Property(template => template.UpdatedAt).IsRequired();
        builder
            .HasIndex(template => new
            {
                template.DocumentType,
                template.Language,
                template.Name,
            })
            .IsUnique();
        builder.HasConcurrencyToken();
    }
}
