using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class WorkspacePreferencesConfiguration
    : IEntityTypeConfiguration<WorkspacePreferencesRow>
{
    public void Configure(EntityTypeBuilder<WorkspacePreferencesRow> builder)
    {
        builder.ToTable("workspace_preferences", "workspace");
        builder.HasKey(preferences => preferences.Id);
        builder.Property(preferences => preferences.Language).HasMaxLength(10).IsRequired();
        builder.Property(preferences => preferences.TimeZone).HasMaxLength(100).IsRequired();
    }
}
