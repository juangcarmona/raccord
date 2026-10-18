using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class IssuerIdentityChangeConfiguration
    : IEntityTypeConfiguration<IssuerIdentityChangeRow>
{
    public void Configure(EntityTypeBuilder<IssuerIdentityChangeRow> builder)
    {
        builder.ToTable("issuer_identity_changes", "workspace");
        builder.HasKey(request => request.Id);
        builder.Property(request => request.OldLegalName).HasMaxLength(200);
        builder.Property(request => request.OldTaxId).HasMaxLength(32);
        builder.Property(request => request.ProposedLegalName).HasMaxLength(200);
        builder.Property(request => request.ProposedTaxId).HasMaxLength(32);
        builder.Property(request => request.Reason).HasMaxLength(2000);
        builder.Property(request => request.RequestedById).HasMaxLength(100);
        builder.Property(request => request.RequestedByName).HasMaxLength(256);
        builder.Property(request => request.Status).HasMaxLength(16);
        builder.Property(request => request.DecidedById).HasMaxLength(100);
        builder.Property(request => request.DecidedByName).HasMaxLength(256);
        builder.HasIndex(request => new { request.Status, request.RequestedAt });
        builder
            .HasIndex(request => request.IssuerId)
            .IsUnique()
            .HasFilter("\"status\" = 'pending'");
        builder.HasConcurrencyToken();
    }
}
