using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Organization;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class FinancialSettingsConfiguration : IEntityTypeConfiguration<FinancialSettings>
{
    public void Configure(EntityTypeBuilder<FinancialSettings> builder)
    {
        builder.ToTable("financial_settings");
        builder.HasKey(settings => settings.Id);
        builder.Property(settings => settings.QuoteValidityDays).HasDefaultValue(30);
        builder.Property(settings => settings.ContractValidityDays).HasDefaultValue(30);
        builder.Property(settings => settings.AllowSigningAfterExpiry).HasDefaultValue(false);
        builder.HasConcurrencyToken();
    }
}
