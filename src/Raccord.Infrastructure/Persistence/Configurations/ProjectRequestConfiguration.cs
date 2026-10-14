using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ProjectRequestConfiguration : IEntityTypeConfiguration<ProjectRequest>
{
    public void Configure(EntityTypeBuilder<ProjectRequest> builder)
    {
        builder.ToTable("project_requests");
        builder.HasKey(request => request.Id);
        builder.Property(request => request.Name).HasMaxLength(200).IsRequired();
        builder.Property(request => request.RequesterName).HasMaxLength(200).IsRequired();
        builder.Property(request => request.RequesterEmail).HasMaxLength(320).IsRequired();
        builder.Property(request => request.Description).HasMaxLength(2000);
        builder
            .Property(request => request.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(request => request.DeclineReason).HasMaxLength(1000);
        builder.HasIndex(request => request.Status);
        builder.HasConcurrencyToken();
    }
}
