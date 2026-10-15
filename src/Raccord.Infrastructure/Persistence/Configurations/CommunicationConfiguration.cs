using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class CommunicationConfiguration : IEntityTypeConfiguration<CommunicationRow>
{
    public void Configure(EntityTypeBuilder<CommunicationRow> builder)
    {
        builder.ToTable("communications", "documents");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.Recipient).HasMaxLength(320).IsRequired();
        builder.Property(row => row.Subject).HasMaxLength(300).IsRequired();
        builder.Property(row => row.DocumentKey).HasMaxLength(300);
        builder.Property(row => row.Status).HasMaxLength(32).IsRequired();
        builder.Property(row => row.FailureReason).HasMaxLength(1000);
        builder.HasIndex(row => row.Recipient);
        builder.HasIndex(row => row.CreatedAt);
    }
}
