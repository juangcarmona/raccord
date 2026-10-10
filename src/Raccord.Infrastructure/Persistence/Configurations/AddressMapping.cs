using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Parties;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal static class AddressMapping
{
    public static void Configure(ComplexPropertyBuilder<AddressFields> address)
    {
        address.Property(fields => fields.Street).HasMaxLength(200);
        address.Property(fields => fields.PostalCode).HasMaxLength(20);
        address.Property(fields => fields.City).HasMaxLength(100);
        address.Property(fields => fields.Province).HasMaxLength(100);
        address.Property(fields => fields.CountryCode).HasMaxLength(2);
    }
}
