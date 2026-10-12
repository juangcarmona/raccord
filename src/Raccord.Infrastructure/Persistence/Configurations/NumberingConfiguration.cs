using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Billing;
using Raccord.Infrastructure.Persistence;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class NumberingConfiguration : IEntityTypeConfiguration<InvoiceNumberSequence>
{
    public void Configure(EntityTypeBuilder<InvoiceNumberSequence> builder)
    {
        builder.ToTable("invoice_number_sequences");
        builder.HasKey(sequence => new { sequence.SeriesCode, sequence.Year });
        builder.Property(sequence => sequence.SeriesCode).HasMaxLength(10);
        builder.HasConcurrencyToken();
    }
}
