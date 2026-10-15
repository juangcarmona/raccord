using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Contracts.Crew;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable MA0048

internal sealed class ProjectFunctionRow
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public CrewFunctionKind Kind { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int RequiredResources { get; set; }
    public List<ProjectShiftRow> Shifts { get; set; } = [];
    public List<CrewAssignmentRow> Crew { get; set; } = [];
    public List<VehicleAssignmentRow> Vehicles { get; set; } = [];
}

internal sealed class ProjectShiftRow
{
    public Guid Id { get; set; }
    public Guid ProjectFunctionRowId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}

internal sealed class CrewAssignmentRow
{
    public Guid Id { get; set; }
    public Guid ProjectFunctionRowId { get; set; }
    public Guid CrewMemberId { get; set; }
    public Guid? RateId { get; set; }
    public bool IncludeTravel { get; set; }
    public bool IsProjectLeader { get; set; }
    public string? Remark { get; set; }
}

internal sealed class VehicleAssignmentRow
{
    public Guid Id { get; set; }
    public Guid ProjectFunctionRowId { get; set; }
    public Guid VehicleId { get; set; }
    public string? Remark { get; set; }
}

internal sealed class ProjectFunctionConfiguration : IEntityTypeConfiguration<ProjectFunctionRow>
{
    public void Configure(EntityTypeBuilder<ProjectFunctionRow> builder)
    {
        builder.ToTable("project_functions", "crew");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.Id).ValueGeneratedNever();
        builder.Property(row => row.Name).HasMaxLength(200);
        builder.Property(row => row.Kind).HasConversion<string>().HasMaxLength(20);
        builder.HasIndex(row => new { row.ProjectId, row.Name }).IsUnique();
        builder
            .HasMany(row => row.Shifts)
            .WithOne()
            .HasForeignKey(row => row.ProjectFunctionRowId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasMany(row => row.Crew)
            .WithOne()
            .HasForeignKey(row => row.ProjectFunctionRowId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasMany(row => row.Vehicles)
            .WithOne()
            .HasForeignKey(row => row.ProjectFunctionRowId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

internal sealed class ProjectShiftConfiguration : IEntityTypeConfiguration<ProjectShiftRow>
{
    public void Configure(EntityTypeBuilder<ProjectShiftRow> builder)
    {
        builder.ToTable("project_shifts", "crew");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.Id).ValueGeneratedNever();
        builder.HasIndex(row => new { row.ProjectFunctionRowId, row.Date });
    }
}

internal sealed class CrewAssignmentConfiguration : IEntityTypeConfiguration<CrewAssignmentRow>
{
    public void Configure(EntityTypeBuilder<CrewAssignmentRow> builder)
    {
        builder.ToTable("project_crew_assignments", "crew");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.Id).ValueGeneratedNever();
        builder.Property(row => row.Remark).HasMaxLength(500);
        builder.HasIndex(row => new { row.ProjectFunctionRowId, row.CrewMemberId }).IsUnique();
    }
}

internal sealed class VehicleAssignmentConfiguration
    : IEntityTypeConfiguration<VehicleAssignmentRow>
{
    public void Configure(EntityTypeBuilder<VehicleAssignmentRow> builder)
    {
        builder.ToTable("project_vehicle_assignments", "crew");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.Id).ValueGeneratedNever();
        builder.Property(row => row.Remark).HasMaxLength(500);
        builder.HasIndex(row => new { row.ProjectFunctionRowId, row.VehicleId }).IsUnique();
    }
}

#pragma warning restore MA0048
