using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ProjectTemplateConfiguration : IEntityTypeConfiguration<ProjectTemplate>
{
    public void Configure(EntityTypeBuilder<ProjectTemplate> builder)
    {
        builder.ToTable("project_templates");
        builder.HasKey(template => template.Id);
        builder.Property(template => template.Name).HasMaxLength(200).IsRequired();
        builder.HasIndex(template => template.Name).IsUnique();
    }
}
