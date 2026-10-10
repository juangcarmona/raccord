using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Organization;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class IssuerProfileConfiguration : IEntityTypeConfiguration<IssuerProfile>
{
    public void Configure(EntityTypeBuilder<IssuerProfile> builder)
    {
        builder.ToTable("issuer_profiles");
        builder.HasKey(profile => profile.Id);
        builder.Property(profile => profile.LegalName).HasMaxLength(200);
        builder.Property(profile => profile.TaxId).HasMaxLength(32);
        builder.ComplexProperty(profile => profile.Address, AddressMapping.Configure);
        builder.HasConcurrencyToken();
    }
}
