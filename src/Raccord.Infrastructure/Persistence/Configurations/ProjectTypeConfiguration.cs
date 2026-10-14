using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ProjectTypeConfiguration : IEntityTypeConfiguration<ProjectType>
{
    public void Configure(EntityTypeBuilder<ProjectType> builder)
    {
        builder.ToTable("project_types");
        builder.HasKey(type => type.Id);
        builder.Property(type => type.Name).HasMaxLength(100).IsRequired();
        builder.Property(type => type.Description).HasMaxLength(500);
        builder.HasIndex(type => type.Name).IsUnique();
    }
}
