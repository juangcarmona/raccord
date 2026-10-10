using Raccord.Domain.Common;
using Raccord.Domain.Customers;
using Raccord.Domain.Organization;
using Raccord.Domain.Parties;

namespace Raccord.Domain.Tests.Parties;

public sealed class PartiesTests
{
    private static readonly PostalAddress Address = new(
        "Avenida del Puerto 20",
        "46023",
        "Valencia",
        "Valencia",
        "ES"
    );

    [Theory]
    [InlineData(" a-00000000 ", "A00000000")]
    [InlineData("es b 0000 0000", "ESB00000000")]
    public void Tax_ids_Should_be_normalized_to_uppercase_without_separators(
        string raw,
        string expected
    )
    {
        var customer = Customer.Register("Productora Ejemplo S.A.", raw, Address, null);

        customer.TaxId.ShouldBe(expected);
    }

    [Fact]
    public void A_customer_without_tax_id_Should_be_registrable_but_not_invoiceable_yet()
    {
        var customer = Customer.Register(
            "Productora Ejemplo S.A.",
            "  ",
            null,
            "billing@example.test"
        );

        customer.TaxId.ShouldBeNull();
        customer.ToLegalParty().ShouldBe(new LegalParty("Productora Ejemplo S.A.", null, null));
    }

    [Fact]
    public void Customers_Should_require_a_legal_name()
    {
        var error = Should.Throw<BusinessRuleViolationException>(() =>
            Customer.Register(" ", "A00000000", Address, null)
        );

        error.Code.ShouldBe("customers.legal-name-required");
    }

    [Fact]
    public void The_issuer_profile_Should_require_name_tax_id_and_complete_address()
    {
        var missingTaxId = Should.Throw<BusinessRuleViolationException>(() =>
            IssuerProfile.Define("Fotograma Rental S.L.", "", Address)
        );
        var badAddress = Should.Throw<BusinessRuleViolationException>(() =>
            IssuerProfile.Define("Fotograma Rental S.L.", "B00000000", Address with { City = "" })
        );

        missingTaxId.Code.ShouldBe("issuer.tax-id-missing");
        badAddress.Code.ShouldBe("issuer.address-missing");
    }

    [Fact]
    public void Updating_the_issuer_profile_Should_not_change_its_identity()
    {
        var profile = IssuerProfile.Define("Fotograma Rental S.L.", "B00000000", Address);
        var id = profile.Id;

        profile.Update(
            "Fotograma Rental, S.L.",
            "b-00000000",
            Address with
            {
                Street = "Calle Nueva 3",
            }
        );

        profile.Id.ShouldBe(id);
        profile
            .ToLegalParty()
            .ShouldBe(
                new LegalParty(
                    "Fotograma Rental, S.L.",
                    "B00000000",
                    Address with
                    {
                        Street = "Calle Nueva 3",
                    }
                )
            );
    }
}
