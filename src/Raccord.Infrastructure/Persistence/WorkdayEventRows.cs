using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Contracts.Crew;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable MA0048
internal sealed class WorkdayEventRow
{
    public Guid Id { get; set; }
    public Guid CrewMemberId { get; set; }
    public WorkdayEventKind Kind { get; set; }
    public DateTimeOffset OccurredAt { get; set; }
    public string Actor { get; set; } = string.Empty;
    public DateTimeOffset RecordedAt { get; set; }
    public string? Remark { get; set; }
}

internal sealed class WorkdayEventConfiguration : IEntityTypeConfiguration<WorkdayEventRow>
{
    public void Configure(EntityTypeBuilder<WorkdayEventRow> builder)
    {
        builder.ToTable("workday_events", "crew");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.Id).ValueGeneratedNever();
        builder.Property(row => row.Kind).HasConversion<string>().HasMaxLength(20);
        builder.Property(row => row.Actor).HasMaxLength(200);
        builder.Property(row => row.Remark).HasMaxLength(500);
        builder.HasIndex(row => new { row.CrewMemberId, row.OccurredAt });
    }
}
#pragma warning restore MA0048
