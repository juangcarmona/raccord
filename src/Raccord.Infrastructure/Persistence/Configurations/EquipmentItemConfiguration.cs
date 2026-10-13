using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class EquipmentItemConfiguration : IEntityTypeConfiguration<EquipmentItemRow>
{
    public void Configure(EntityTypeBuilder<EquipmentItemRow> builder)
    {
        builder.ToTable("items", "equipment");
        builder.HasKey(item => item.Id);
        builder.HasIndex(item => item.Code).IsUnique();
        builder.Property(item => item.Code).HasMaxLength(100).IsRequired();
        builder.Property(item => item.Name).HasMaxLength(200).IsRequired();
        builder.Property(item => item.SerialNumbers).HasColumnType("jsonb").IsRequired();
        builder.Property(item => item.AccessoryIds).HasColumnType("jsonb").IsRequired();
        builder.Property(item => item.AlternativeIds).HasColumnType("jsonb").IsRequired();
        builder.Property(item => item.Details).HasMaxLength(5000);
        builder.Property(item => item.ImageKey).HasMaxLength(300);
    }
}
