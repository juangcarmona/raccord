using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ProjectTeamConfiguration : IEntityTypeConfiguration<ProjectTeam>
{
    public void Configure(EntityTypeBuilder<ProjectTeam> builder)
    {
        builder.ToTable("project_teams");
        builder.HasKey(team => team.Id);
        builder.Property(team => team.Name).HasMaxLength(100).IsRequired();
        builder.Property(team => team.Description).HasMaxLength(500);
        builder.HasIndex(team => team.Name).IsUnique();
    }
}
