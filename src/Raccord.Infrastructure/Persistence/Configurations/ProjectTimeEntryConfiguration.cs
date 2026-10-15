using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ProjectTimeEntryConfiguration : IEntityTypeConfiguration<ProjectTimeEntry>
{
    public void Configure(EntityTypeBuilder<ProjectTimeEntry> builder)
    {
        builder.ToTable("project_time_entries", "projects");
        builder.HasKey(entry => entry.Id);
        builder.Property(entry => entry.Service).HasMaxLength(300).IsRequired();
        builder.Property(entry => entry.PlannedHours).HasPrecision(10, 2);
        builder.Property(entry => entry.ActualHours).HasPrecision(10, 2);
        builder.Property(entry => entry.Actor).HasMaxLength(200).IsRequired();
        builder.HasIndex(entry => new { entry.ProjectId, entry.RecordedAt });
        builder.HasIndex(entry => entry.TaskId);
    }
}
