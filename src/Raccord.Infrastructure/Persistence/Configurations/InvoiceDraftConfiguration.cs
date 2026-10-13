using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Billing;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class InvoiceDraftConfiguration : IEntityTypeConfiguration<InvoiceDraft>
{
    public void Configure(EntityTypeBuilder<InvoiceDraft> builder)
    {
        builder.ToTable("invoice_drafts");
        builder.HasKey(draft => draft.Id);
        builder.Property(draft => draft.SeriesCode).HasMaxLength(10);
        builder.Property(draft => draft.Status).HasConversion<string>().HasMaxLength(20);
        builder
            .HasOne<Project>()
            .WithMany()
            .HasForeignKey(draft => draft.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(draft => draft.Status);
        builder.HasConcurrencyToken();

        builder.OwnsMany(
            draft => draft.Lines,
            line =>
            {
                line.ToTable("invoice_draft_lines");
                line.WithOwner().HasForeignKey("DraftId");
                line.HasKey(item => item.Id);
                line.Property(item => item.Id).ValueGeneratedNever();
                line.Property(item => item.Description).HasMaxLength(500);
                line.Property(item => item.Quantity).HasPrecision(18, 4);
                line.Property(item => item.UnitPrice).HasPrecision(18, 4);
                line.Property(item => item.DiscountPercent).HasPrecision(5, 2);
                line.Property(item => item.VatPercentage).HasPrecision(5, 2);
                line.Property(item => item.CurrencyCode).HasMaxLength(3);
                line.Property(item => item.IsAdjustment);
            }
        );
        builder.Navigation(draft => draft.Lines).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
