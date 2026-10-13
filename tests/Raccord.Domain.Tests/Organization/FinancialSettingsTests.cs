using Raccord.Domain.Organization;
using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Tests.Organization;

public sealed class FinancialSettingsTests
{
    [Fact]
    public void Settings_Should_require_positive_validity_periods()
    {
        var error = Should.Throw<BusinessRuleViolationException>(() =>
            FinancialSettings.Configure(0, 30, false)
        );

        error.Code.ShouldBe("financial-settings.validity-positive");
    }

    [Fact]
    public void Settings_Should_update_expiry_policy_without_changing_other_values()
    {
        var settings = FinancialSettings.Configure(30, 60, false);

        settings.Update(45, 90, true);

        settings.QuoteValidityDays.ShouldBe(45);
        settings.ContractValidityDays.ShouldBe(90);
        settings.AllowSigningAfterExpiry.ShouldBeTrue();
    }
}
