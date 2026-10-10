using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Customers;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");
        builder.HasKey(customer => customer.Id);
        builder.Property(customer => customer.LegalName).HasMaxLength(200);
        builder.Property(customer => customer.TaxId).HasMaxLength(32);
        builder.Property(customer => customer.Email).HasMaxLength(320);
        builder.ComplexProperty(customer => customer.BillingAddress, AddressMapping.Configure);
        builder.HasIndex(customer => customer.TaxId);
    }
}
