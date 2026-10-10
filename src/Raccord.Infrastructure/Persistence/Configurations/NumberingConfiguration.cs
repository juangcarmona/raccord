using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Billing;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class NumberingConfiguration : IEntityTypeConfiguration<InvoiceSeries>, IEntityTypeConfiguration<InvoiceNumberSequence>
{
    public void Configure(EntityTypeBuilder<InvoiceSeries> builder)
    {
        builder.ToTable("invoice_series");
        builder.HasKey(series => series.Code);
        builder.Property(series => series.Code).HasMaxLength(10);
        builder.Property(series => series.Description).HasMaxLength(200);
    }

    public void Configure(EntityTypeBuilder<InvoiceNumberSequence> builder)
    {
        builder.ToTable("invoice_number_sequences");
        builder.HasKey(sequence => new { sequence.SeriesCode, sequence.Year });
        builder.Property(sequence => sequence.SeriesCode).HasMaxLength(10);
        builder.HasOne<InvoiceSeries>().WithMany().HasForeignKey(sequence => sequence.SeriesCode).OnDelete(DeleteBehavior.Restrict);
        builder.HasConcurrencyToken();
    }
}
