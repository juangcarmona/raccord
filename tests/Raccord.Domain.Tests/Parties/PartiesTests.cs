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
    public void Tax_ids_are_normalized_to_uppercase_without_separators(string raw, string expected)
    {
        var customer = Customer.Register("Productora Ejemplo S.A.", raw, Address, null);

        Assert.Equal(expected, customer.TaxId);
    }

    [Fact]
    public void A_customer_without_tax_id_can_be_registered_but_is_not_invoiceable_yet()
    {
        var customer = Customer.Register(
            "Productora Ejemplo S.A.",
            "  ",
            null,
            "billing@example.test"
        );

        Assert.Null(customer.TaxId);
        Assert.Equal(
            new LegalParty("Productora Ejemplo S.A.", null, null),
            customer.ToLegalParty()
        );
    }

    [Fact]
    public void Customers_need_a_legal_name()
    {
        var error = Assert.Throws<BusinessRuleViolationException>(() =>
            Customer.Register(" ", "A00000000", Address, null)
        );

        Assert.Equal("customers.legal-name-required", error.Code);
    }

    [Fact]
    public void The_issuer_profile_requires_name_tax_id_and_complete_address()
    {
        var missingTaxId = Assert.Throws<BusinessRuleViolationException>(() =>
            IssuerProfile.Define("Fotograma Rental S.L.", "", Address)
        );
        var badAddress = Assert.Throws<BusinessRuleViolationException>(() =>
            IssuerProfile.Define("Fotograma Rental S.L.", "B00000000", Address with { City = "" })
        );

        Assert.Equal("issuer.tax-id-missing", missingTaxId.Code);
        Assert.Equal("issuer.address-missing", badAddress.Code);
    }

    [Fact]
    public void Updating_the_issuer_profile_does_not_change_its_identity()
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

        Assert.Equal(id, profile.Id);
        Assert.Equal(
            new LegalParty(
                "Fotograma Rental, S.L.",
                "B00000000",
                Address with
                {
                    Street = "Calle Nueva 3",
                }
            ),
            profile.ToLegalParty()
        );
    }
}
