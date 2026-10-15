using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence;

internal sealed class CalendarFeedConfiguration : IEntityTypeConfiguration<CalendarFeedRow>
{
    public void Configure(EntityTypeBuilder<CalendarFeedRow> builder)
    {
        builder.ToTable("calendar_feeds", "workspace");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.Id).ValueGeneratedNever();
        builder.Property(row => row.TokenHash).HasMaxLength(64).IsRequired();
        builder.Property(row => row.Stream).HasMaxLength(40).IsRequired();
        builder.HasIndex(row => row.TokenHash).IsUnique();
    }
}
