using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Equipment;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("stock_movements", "equipment");
        builder.HasKey(movement => movement.Id);
        builder.Property(movement => movement.Direction).HasConversion<int>().IsRequired();
        builder.Property(movement => movement.Status).HasConversion<int>().IsRequired();
        builder.Property(movement => movement.Reference).HasMaxLength(200);
        builder.Property(movement => movement.Quantity).IsRequired();
        builder.HasIndex(movement => new { movement.EquipmentId, movement.ScheduledDate });
    }
}
