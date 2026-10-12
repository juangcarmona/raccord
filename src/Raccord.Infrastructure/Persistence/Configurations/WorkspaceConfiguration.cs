using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class WorkspaceConfiguration : IEntityTypeConfiguration<InvoiceSeriesRow>
{
    public void Configure(EntityTypeBuilder<InvoiceSeriesRow> builder)
    {
        builder.ToTable("invoice_series", "workspace");
        builder.HasKey(series => series.Code);
        builder.Property(series => series.Code).HasMaxLength(10);
        builder.Property(series => series.Description).HasMaxLength(200);
    }
}
