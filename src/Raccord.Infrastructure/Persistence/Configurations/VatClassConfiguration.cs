using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Billing;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class VatClassConfiguration : IEntityTypeConfiguration<VatClass>
{
    public void Configure(EntityTypeBuilder<VatClass> builder)
    {
        builder.ToTable("vat_classes", "invoicing");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).ValueGeneratedNever();
        builder.Property(item => item.Name).HasMaxLength(100).IsRequired();
        builder.HasIndex(item => item.Name).IsUnique();
    }
}
