using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class LetterheadConfiguration : IEntityTypeConfiguration<LetterheadRow>
{
    public void Configure(EntityTypeBuilder<LetterheadRow> builder)
    {
        builder.ToTable("letterheads", "documents");
        builder.HasKey(letterhead => letterhead.Id);
        builder.HasIndex(letterhead => letterhead.Name).IsUnique();
        builder.Property(letterhead => letterhead.Name).HasMaxLength(200).IsRequired();
        builder.Property(letterhead => letterhead.DocumentTypes).HasMaxLength(1000).IsRequired();
        builder.Property(letterhead => letterhead.StorageKey).HasMaxLength(300).IsRequired();
        builder.Property(letterhead => letterhead.PageWidthMillimetres).HasPrecision(8, 2);
        builder.Property(letterhead => letterhead.PageHeightMillimetres).HasPrecision(8, 2);
        builder.Property(letterhead => letterhead.MarginTopMillimetres).HasPrecision(8, 2);
        builder.Property(letterhead => letterhead.MarginRightMillimetres).HasPrecision(8, 2);
        builder.Property(letterhead => letterhead.MarginBottomMillimetres).HasPrecision(8, 2);
        builder.Property(letterhead => letterhead.MarginLeftMillimetres).HasPrecision(8, 2);
    }
}
