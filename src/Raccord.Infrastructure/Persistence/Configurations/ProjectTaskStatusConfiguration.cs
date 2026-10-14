using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ProjectTaskStatusConfiguration : IEntityTypeConfiguration<ProjectTaskStatus>
{
    public void Configure(EntityTypeBuilder<ProjectTaskStatus> builder)
    {
        builder.ToTable("project_task_statuses");
        builder.HasKey(status => status.Id);
        builder.Property(status => status.Name).HasMaxLength(100).IsRequired();
        builder.HasIndex(status => status.Position).IsUnique();
    }
}
