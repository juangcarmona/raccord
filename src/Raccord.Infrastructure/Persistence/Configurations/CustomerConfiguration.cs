using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Infrastructure.Persistence;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<CustomerRow>
{
    public void Configure(EntityTypeBuilder<CustomerRow> builder)
    {
        builder.ToTable("customers", "contacts");
        builder.HasKey(customer => customer.Id);
        builder.Property(customer => customer.LegalName).HasMaxLength(200);
        builder.Property(customer => customer.Type).HasConversion<string>().HasMaxLength(32);
        builder.Property(customer => customer.TaxId).HasMaxLength(32);
        builder.Property(customer => customer.Email).HasMaxLength(320);
        builder.Property(customer => customer.DefaultDiscountPercent).HasPrecision(5, 2);
        builder.Property(customer => customer.DefaultPaymentTerms).HasMaxLength(200);
        builder.Property(customer => customer.Tags).HasColumnType("jsonb");
        builder.Property(customer => customer.Folder).HasMaxLength(100);
        builder.ComplexProperty(customer => customer.BillingAddress, AddressMapping.Configure);
        builder.HasIndex(customer => customer.TaxId);
    }
}
