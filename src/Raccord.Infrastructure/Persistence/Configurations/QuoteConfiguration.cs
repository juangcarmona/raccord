using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Commercial;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class QuoteConfiguration
    : IEntityTypeConfiguration<Quote>,
        IEntityTypeConfiguration<QuoteNumberSequence>
{
    public void Configure(EntityTypeBuilder<Quote> builder)
    {
        builder.ToTable("quotes");
        builder.HasKey(quote => quote.Id);
        builder.Property(quote => quote.Number).HasMaxLength(30).IsRequired();
        builder.Property(quote => quote.SeriesCode).HasMaxLength(10).IsRequired();
        builder.Property(quote => quote.CustomerName).HasMaxLength(200).IsRequired();
        builder.Property(quote => quote.ProjectName).HasMaxLength(200).IsRequired();
        builder.Property(quote => quote.PaymentTerms).HasMaxLength(200);
        builder
            .Property(quote => quote.ReviewStatus)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();
        builder.HasIndex(quote => quote.Number).IsUnique();
        builder.OwnsMany(
            quote => quote.Lines,
            line =>
            {
                line.ToTable("quote_lines");
                line.WithOwner().HasForeignKey("QuoteId");
                line.HasKey(item => item.Id);
                line.Property(item => item.Id).ValueGeneratedNever();
                line.Property(item => item.Description).HasMaxLength(500).IsRequired();
                line.Property(item => item.CurrencyCode).HasMaxLength(3).IsRequired();
                line.Property(item => item.Quantity).HasPrecision(18, 4);
                line.Property(item => item.UnitPrice).HasPrecision(18, 4);
                line.Property(item => item.DiscountPercent).HasPrecision(5, 2);
                line.Property(item => item.VatPercentage).HasPrecision(5, 2);
                line.HasIndex("QuoteId", nameof(QuoteLine.Position)).IsUnique();
            }
        );
        builder.Navigation(quote => quote.Lines).UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    public void Configure(EntityTypeBuilder<QuoteNumberSequence> builder)
    {
        builder.ToTable("quote_number_sequences");
        builder.HasKey(sequence => new { sequence.SeriesCode, sequence.Year });
        builder.Property(sequence => sequence.SeriesCode).HasMaxLength(10);
    }
}
