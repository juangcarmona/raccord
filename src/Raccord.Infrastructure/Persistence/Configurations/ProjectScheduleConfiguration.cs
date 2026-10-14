using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ProjectScheduleConfiguration : IEntityTypeConfiguration<ProjectSchedule>
{
    public void Configure(EntityTypeBuilder<ProjectSchedule> builder)
    {
        builder.ToTable("project_schedules");
        builder.HasKey(schedule => schedule.Id);
        builder.Property(schedule => schedule.Title).HasMaxLength(200).IsRequired();
        builder.HasIndex(schedule => new
        {
            schedule.AssigneeId,
            schedule.StartDate,
            schedule.EndDate,
        });
    }
}
