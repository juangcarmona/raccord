using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Contracts.Crew;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable MA0048

internal sealed class CrewAvailabilityRow
{
    public Guid Id { get; set; }
    public Guid CrewMemberId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public CrewAvailabilityKind Kind { get; set; }
    public string? Remark { get; set; }
}

internal sealed class CrewInvitationRow
{
    public Guid Id { get; set; }
    public Guid FunctionId { get; set; }
    public Guid CrewMemberId { get; set; }
    public CrewInvitationStatus Status { get; set; }
    public string? Remark { get; set; }
}

internal sealed class CrewJobRow
{
    public Guid Id { get; set; }
    public Guid FunctionId { get; set; }
    public string? Brief { get; set; }
    public DateTimeOffset PostedAt { get; set; }
    public List<CrewSignupRow> Signups { get; set; } = [];
}

internal sealed class CrewSignupRow
{
    public Guid Id { get; set; }
    public Guid JobId { get; set; }
    public Guid CrewMemberId { get; set; }
    public DateTimeOffset SignedUpAt { get; set; }
}

internal sealed class CrewAvailabilityConfiguration : IEntityTypeConfiguration<CrewAvailabilityRow>
{
    public void Configure(EntityTypeBuilder<CrewAvailabilityRow> builder)
    {
        builder.ToTable("crew_availability", "crew");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.Id).ValueGeneratedNever();
        builder.Property(row => row.Kind).HasConversion<string>().HasMaxLength(20);
        builder.Property(row => row.Remark).HasMaxLength(500);
    }
}

internal sealed class CrewInvitationConfiguration : IEntityTypeConfiguration<CrewInvitationRow>
{
    public void Configure(EntityTypeBuilder<CrewInvitationRow> builder)
    {
        builder.ToTable("crew_invitations", "crew");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.Id).ValueGeneratedNever();
        builder.Property(row => row.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(row => row.Remark).HasMaxLength(500);
        builder.HasIndex(row => new { row.FunctionId, row.CrewMemberId }).IsUnique();
    }
}

internal sealed class CrewJobConfiguration : IEntityTypeConfiguration<CrewJobRow>
{
    public void Configure(EntityTypeBuilder<CrewJobRow> builder)
    {
        builder.ToTable("crew_jobs", "crew");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.Id).ValueGeneratedNever();
        builder.Property(row => row.Brief).HasMaxLength(1000);
        builder
            .HasMany(row => row.Signups)
            .WithOne()
            .HasForeignKey(row => row.JobId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

internal sealed class CrewSignupConfiguration : IEntityTypeConfiguration<CrewSignupRow>
{
    public void Configure(EntityTypeBuilder<CrewSignupRow> builder)
    {
        builder.ToTable("crew_signups", "crew");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.Id).ValueGeneratedNever();
        builder.HasIndex(row => new { row.JobId, row.CrewMemberId }).IsUnique();
    }
}

#pragma warning restore MA0048
