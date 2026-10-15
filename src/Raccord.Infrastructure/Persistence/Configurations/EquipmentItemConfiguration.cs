using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Equipment;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class EquipmentItemConfiguration : IEntityTypeConfiguration<EquipmentItem>
{
    public void Configure(EntityTypeBuilder<EquipmentItem> builder)
    {
        builder.ToTable("equipment_items", "equipment");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Code).HasMaxLength(100).IsRequired();
        builder.Property(item => item.Name).HasMaxLength(200).IsRequired();
        builder.Property(item => item.ScanCode).HasMaxLength(200);
        builder.HasIndex(item => item.Code).IsUnique();
        builder.HasIndex(item => item.ScanCode).IsUnique();
        builder.Property(item => item.StockQuantity).IsRequired();
        builder.Property(item => item.AvailableQuantity).IsRequired();
        builder.HasConcurrencyToken();
    }
}
