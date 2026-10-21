using Raccord.Domain.Billing;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Tests.Billing;

public sealed class VatSchemeTests
{
    [Fact]
    public void Spanish_standard_rate_bootstrap_Should_keep_the_source_and_verification_date()
    {
        var bootstrap = SpanishVatDefaults.StandardRate;

        bootstrap.Territory.ShouldBe("ES");
        bootstrap.Percentage.ShouldBe(21m);
        bootstrap.SourceUrl.ShouldBe(
            "https://sede.agenciatributaria.gob.es/Sede/ayuda/manuales-videos-folletos/manuales-practicos/manual-iva-2026/capitulo-04-sujetos-pasivos-repercusion-impositivo/tipo-impositivo.html"
        );
        bootstrap.VerifiedOn.ShouldBe(new DateOnly(2026, 10, 21));
    }

    [Fact]
    public void Vat_class_Should_trim_its_name_and_keep_its_identity_when_renamed()
    {
        var vatClass = VatClass.Create("  Standard rate  ");
        var id = vatClass.Id;

        vatClass.Rename("General rate");

        vatClass.Name.ShouldBe("General rate");
        vatClass.Id.ShouldBe(id);
    }

    [Fact]
    public void Vat_class_Should_reject_an_empty_name()
    {
        var error = Should.Throw<BusinessRuleViolationException>(() => VatClass.Create("  "));

        error.Code.ShouldBe("invoicing.vat-class-name-invalid");
    }

    [Fact]
    public void Per_class_scheme_Should_resolve_rate_by_stable_class_identity()
    {
        var standardClassId = Guid.CreateVersion7();
        var reducedClassId = Guid.CreateVersion7();
        var scheme = VatScheme.CreatePerClass(
            "Domestic rates",
            [new VatClassRate(standardClassId, 21m), new VatClassRate(reducedClassId, 10m)]
        );

        scheme.RateFor(standardClassId).Percentage.ShouldBe(21m);
        scheme.RateFor(reducedClassId).Percentage.ShouldBe(10m);
        scheme.Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public void Per_class_scheme_Should_reject_unmapped_class_identity()
    {
        var scheme = VatScheme.CreatePerClass(
            "Domestic rates",
            [new VatClassRate(Guid.CreateVersion7(), 21m)]
        );

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            scheme.RateFor(Guid.CreateVersion7())
        );

        error.Code.ShouldBe("invoicing.vat-class-unmapped");
    }

    [Fact]
    public void Fixed_rate_scheme_Should_apply_its_rate_to_a_selected_class()
    {
        var scheme = VatScheme.CreateFixedRate("Customer-specific rate", 7.5m);

        scheme.RateFor(Guid.CreateVersion7()).Percentage.ShouldBe(7.5m);
    }

    [Fact]
    public void Reverse_charge_scheme_Should_resolve_to_zero_calculated_rate()
    {
        var scheme = VatScheme.CreateReverseCharge("Reverse charge");

        scheme.RateFor(Guid.CreateVersion7()).Percentage.ShouldBe(0m);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(100.01)]
    public void Scheme_with_out_of_range_rate_Should_be_rejected(decimal rate)
    {
        var error = Should.Throw<BusinessRuleViolationException>(() =>
            VatScheme.CreateFixedRate("Invalid rate", rate)
        );

        error.Code.ShouldBe("pricing.vat-rate-out-of-range");
    }

    [Fact]
    public void Per_class_scheme_Should_reject_duplicate_class_mappings()
    {
        var classId = Guid.CreateVersion7();

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            VatScheme.CreatePerClass(
                "Duplicate mapping",
                [new VatClassRate(classId, 21m), new VatClassRate(classId, 10m)]
            )
        );

        error.Code.ShouldBe("invoicing.vat-class-rate-duplicate");
    }

    [Fact]
    public void Used_scheme_Should_not_allow_its_rates_to_change()
    {
        var classId = Guid.CreateVersion7();
        var scheme = VatScheme.CreatePerClass("Domestic rates", [new VatClassRate(classId, 21m)]);
        scheme.MarkUsed();

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            scheme.UpdatePerClassRates([new VatClassRate(classId, 23m)])
        );

        error.Code.ShouldBe("invoicing.vat-scheme-used");
        scheme.RateFor(classId).Percentage.ShouldBe(21m);
    }
}
