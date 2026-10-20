using Raccord.Modules.Contacts.Contracts;
using Raccord.Modules.Contacts.Domain;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Parties;

namespace Raccord.Domain.Tests.Parties;

public sealed class ContactTests
{
    private static readonly PostalAddress Address = new(
        "Avenida del Puerto 20",
        "46023",
        "Valencia",
        "Valencia",
        "ES"
    );

    [Fact]
    public void Registering_a_company_with_multiple_roles_Should_preserve_type_and_roles()
    {
        var contact = Contact.Register(
            ContactType.Company,
            " Productora Ejemplo S.A. ",
            "b-00000000",
            Address,
            " billing@example.test ",
            null,
            [ContactRole.Customer, ContactRole.Supplier]
        );

        contact.Type.ShouldBe(ContactType.Company);
        contact.LegalName.ShouldBe("Productora Ejemplo S.A.");
        contact.TaxId.ShouldBe("B00000000");
        contact.Email.ShouldBe("billing@example.test");
        contact.Roles.ShouldBe([ContactRole.Customer, ContactRole.Supplier]);
        contact.ContactNumber.ShouldBeNull();
    }

    [Fact]
    public void Registering_a_private_person_venue_Should_preserve_its_person_type()
    {
        var contact = Contact.Register(
            ContactType.PrivatePerson,
            " Marina Costa ",
            null,
            null,
            null,
            null,
            [ContactRole.Venue]
        );

        contact.Type.ShouldBe(ContactType.PrivatePerson);
        contact.Roles.ShouldBe([ContactRole.Venue]);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void Contacts_without_a_legal_name_Should_be_rejected(string legalName)
    {
        var error = Should.Throw<BusinessRuleViolationException>(() =>
            Contact.Register(
                ContactType.Company,
                legalName,
                null,
                null,
                null,
                null,
                [ContactRole.Customer]
            )
        );

        error.Code.ShouldBe("contacts.legal-name-required");
    }

    [Fact]
    public void Contacts_without_a_supported_role_Should_be_rejected()
    {
        var error = Should.Throw<BusinessRuleViolationException>(() =>
            Contact.Register(
                ContactType.Company,
                "Productora Ejemplo S.A.",
                null,
                null,
                null,
                null,
                []
            )
        );

        error.Code.ShouldBe("contacts.role-required");
    }

    [Fact]
    public void Contacts_with_an_unsupported_role_value_Should_be_rejected()
    {
        var error = Should.Throw<BusinessRuleViolationException>(() =>
            Contact.Register(
                ContactType.Company,
                "Productora Ejemplo S.A.",
                null,
                null,
                null,
                null,
                [(ContactRole)999]
            )
        );

        error.Code.ShouldBe("contacts.role-unsupported");
    }

    [Fact]
    public void Updating_a_contact_Should_keep_its_identifier_and_replace_its_roles()
    {
        var contact = Contact.Register(
            ContactType.Company,
            "Productora Ejemplo S.A.",
            null,
            Address,
            null,
            "10001",
            [ContactRole.Customer]
        );
        var id = contact.Id;

        contact.Update(
            ContactType.PrivatePerson,
            "Marina Costa",
            null,
            Address,
            null,
            "10001",
            [ContactRole.Venue]
        );

        contact.Id.ShouldBe(id);
        contact.Type.ShouldBe(ContactType.PrivatePerson);
        contact.LegalName.ShouldBe("Marina Costa");
        contact.Roles.ShouldBe([ContactRole.Venue]);
    }
}
