using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Contracts.Crew;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable MA0048
internal sealed class CrewHoursRow
{
    public Guid Id { get; set; }
    public Guid CrewMemberId { get; set; }
    public DateOnly WorkDate { get; set; }
    public decimal Hours { get; set; }
    public CrewHourSource Source { get; set; }
    public Guid? FunctionId { get; set; }
    public CrewHourStatus Status { get; set; }
    public string? Remark { get; set; }
}

internal sealed class CrewHoursConfiguration : IEntityTypeConfiguration<CrewHoursRow>
{
    public void Configure(EntityTypeBuilder<CrewHoursRow> builder)
    {
        builder.ToTable("crew_hours", "crew");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.Id).ValueGeneratedNever();
        builder.Property(row => row.Hours).HasPrecision(8, 2);
        builder.Property(row => row.Source).HasConversion<string>().HasMaxLength(30);
        builder.Property(row => row.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(row => row.Remark).HasMaxLength(500);
        builder.HasIndex(row => new { row.CrewMemberId, row.WorkDate });
    }
}
#pragma warning restore MA0048
