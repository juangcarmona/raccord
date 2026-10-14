using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class EquipmentStockMovementConfiguration
    : IEntityTypeConfiguration<EquipmentStockMovementRow>
{
    public void Configure(EntityTypeBuilder<EquipmentStockMovementRow> builder)
    {
        builder.ToTable("stock_movements", "equipment");
        builder.HasKey(movement => movement.Id);
        builder.HasIndex(movement => new { movement.EquipmentItemId, movement.EffectiveDate });
        builder.Property(movement => movement.Kind).HasMaxLength(40).IsRequired();
        builder.Property(movement => movement.Description).HasMaxLength(1000);
        builder.Property(movement => movement.CreatedAt).IsRequired();
    }
}
