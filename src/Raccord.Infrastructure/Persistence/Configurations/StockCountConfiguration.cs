using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Equipment;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class StockCountConfiguration : IEntityTypeConfiguration<StockCount>
{
    public void Configure(EntityTypeBuilder<StockCount> builder)
    {
        builder.ToTable("stock_counts", "equipment");
        builder.HasKey(count => count.Id);
        builder.Property(count => count.Reason).HasMaxLength(500).IsRequired();
        builder.Property(count => count.CountedQuantity).IsRequired();
        builder.Property(count => count.PreviousQuantity).IsRequired();
        builder.Property(count => count.Difference).IsRequired();
        builder.HasIndex(count => new { count.EquipmentId, count.CountedAt });
    }
}
