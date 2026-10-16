using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence;

internal sealed class ApiAccessConfiguration : IEntityTypeConfiguration<ApiAccessRow>
{
    public void Configure(EntityTypeBuilder<ApiAccessRow> builder)
    {
        builder.ToTable("api_access_tokens", "workspace");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.Id).ValueGeneratedNever();
        builder.Property(row => row.Name).HasMaxLength(200);
        builder.Property(row => row.TokenHash).HasMaxLength(128);
        builder.Property(row => row.Scopes).HasMaxLength(2000);
        builder.HasIndex(row => row.TokenHash).IsUnique();
    }
}
