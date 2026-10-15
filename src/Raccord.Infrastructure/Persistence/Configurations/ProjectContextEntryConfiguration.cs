using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ProjectContextEntryConfiguration
    : IEntityTypeConfiguration<ProjectContextEntry>
{
    public void Configure(EntityTypeBuilder<ProjectContextEntry> builder)
    {
        builder.ToTable("project_context_entries", "projects");
        builder.HasKey(entry => entry.Id);
        builder.Property(entry => entry.Kind).HasMaxLength(30).IsRequired();
        builder.Property(entry => entry.Name).HasMaxLength(300).IsRequired();
        builder.Property(entry => entry.Content).HasColumnType("text");
        builder.Property(entry => entry.Folder).HasMaxLength(300);
        builder.Property(entry => entry.Key).HasMaxLength(100);
        builder.Property(entry => entry.Value).HasColumnType("text");
        builder.Property(entry => entry.TargetType).HasMaxLength(50);
        builder.Property(entry => entry.Actor).HasMaxLength(200).IsRequired();
        builder.HasIndex(entry => new
        {
            entry.ProjectId,
            entry.Kind,
            entry.CreatedAt,
        });
        builder.HasIndex(entry => new { entry.ProjectId, entry.Key });
    }
}
