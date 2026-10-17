using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Infrastructure.Persistence;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class IssuerProfileConfiguration : IEntityTypeConfiguration<IssuerProfileRow>
{
    public void Configure(EntityTypeBuilder<IssuerProfileRow> builder)
    {
        builder.ToTable("issuer_profiles", "workspace");
        builder.HasKey(profile => profile.Id);
        builder.Property(profile => profile.LegalName).HasMaxLength(200);
        builder.Property(profile => profile.TaxId).HasMaxLength(32);
        builder.Property(profile => profile.IssuedInvoiceCount).HasDefaultValue(0);
        builder.ComplexProperty(profile => profile.Address, AddressMapping.Configure);
        builder.HasConcurrencyToken();
    }
}
