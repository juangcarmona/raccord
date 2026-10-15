using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Billing;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class InvoicePaymentConfiguration : IEntityTypeConfiguration<InvoicePayment>
{
    public void Configure(EntityTypeBuilder<InvoicePayment> builder)
    {
        builder.ToTable("invoice_payments");
        builder.HasKey(payment => payment.Id);
        builder.Property(payment => payment.Method).HasMaxLength(50);
        builder.Property(payment => payment.Description).HasMaxLength(500);
        builder.Property(payment => payment.Amount).HasPrecision(18, 2);
        builder.HasIndex(payment => payment.InvoiceId);
    }
}
