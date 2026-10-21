using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Billing;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class VatSchemeConfiguration : IEntityTypeConfiguration<VatScheme>
{
    public void Configure(EntityTypeBuilder<VatScheme> builder)
    {
        builder.ToTable("vat_schemes", "invoicing");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).ValueGeneratedNever();
        builder.Property(item => item.Name).HasMaxLength(100).IsRequired();
        builder.Property(item => item.Kind).HasConversion<string>().HasMaxLength(30);
        builder.Property(item => item.FixedPercentage).HasPrecision(5, 2);
        builder.Property(item => item.Territory).HasMaxLength(2);
        builder.Property(item => item.SourceUrl).HasMaxLength(500);
        builder.HasIndex(item => item.Name).IsUnique();
        builder.HasConcurrencyToken();
        builder.Navigation(item => item.ClassRates).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(
            item => item.ClassRates,
            rate =>
            {
                rate.ToTable("vat_scheme_class_rates", "invoicing");
                rate.WithOwner().HasForeignKey("VatSchemeId");
                rate.HasKey("VatSchemeId", nameof(VatClassRate.VatClassId));
                rate.Property(item => item.VatClassId).ValueGeneratedNever();
                rate.Property(item => item.Percentage).HasPrecision(5, 2).IsRequired();
                rate.HasOne<VatClass>().WithMany().HasForeignKey(item => item.VatClassId);
            }
        );
    }
}
