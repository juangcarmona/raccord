using Raccord.Modules.Contacts.Domain;
using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Tests.Parties;

public sealed class ContactPersonTests
{
    [Fact]
    public void Creating_a_contact_person_Should_trim_supported_details_and_keep_designations()
    {
        var contactId = Guid.CreateVersion7();

        var person = ContactPerson.Create(
            contactId,
            " Alex Rivera ",
            " alex@example.test ",
            " +34 600 123 456 "
        );
        ContactPerson.AssignDesignations(person, [person], isDefault: true, isAdministrative: true);

        person.ContactId.ShouldBe(contactId);
        person.Name.ShouldBe("Alex Rivera");
        person.Email.ShouldBe("alex@example.test");
        person.Phone.ShouldBe("+34 600 123 456");
        person.IsDefault.ShouldBeTrue();
        person.IsAdministrative.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void Contact_persons_without_a_name_Should_be_rejected(string name)
    {
        var error = Should.Throw<BusinessRuleViolationException>(() =>
            ContactPerson.Create(Guid.CreateVersion7(), name, null, null)
        );

        error.Code.ShouldBe("contacts.contact-person-name-required");
    }

    [Fact]
    public void Contact_persons_with_an_invalid_email_Should_be_rejected()
    {
        var error = Should.Throw<BusinessRuleViolationException>(() =>
            ContactPerson.Create(Guid.CreateVersion7(), "Alex Rivera", "not-an-email", null)
        );

        error.Code.ShouldBe("contacts.contact-person-email-invalid");
    }

    [Fact]
    public void Updating_a_contact_person_Should_preserve_identity_and_replace_saved_details()
    {
        var contactId = Guid.CreateVersion7();
        var person = ContactPerson.Create(contactId, "Alex Rivera", null, null);
        var id = person.Id;

        person.Update(contactId, " Bea Costa ", " bea@example.test ", " 600 100 200 ");
        ContactPerson.AssignDesignations(
            person,
            [person],
            isDefault: true,
            isAdministrative: false
        );

        person.Id.ShouldBe(id);
        person.ContactId.ShouldBe(contactId);
        person.Name.ShouldBe("Bea Costa");
        person.Email.ShouldBe("bea@example.test");
        person.Phone.ShouldBe("600 100 200");
        person.IsDefault.ShouldBeTrue();
        person.IsAdministrative.ShouldBeFalse();
    }

    [Fact]
    public void Rejected_contact_person_updates_Should_preserve_the_saved_details_and_designations()
    {
        var contactId = Guid.CreateVersion7();
        var person = ContactPerson.Create(
            contactId,
            "Alex Rivera",
            "alex@example.test",
            "+34 600 123 456"
        );
        ContactPerson.AssignDesignations(person, [person], isDefault: true, isAdministrative: true);

        Should.Throw<BusinessRuleViolationException>(() =>
            person.Update(contactId, "Bea Costa", "not-an-email", null)
        );

        person.Name.ShouldBe("Alex Rivera");
        person.Email.ShouldBe("alex@example.test");
        person.Phone.ShouldBe("+34 600 123 456");
        person.IsDefault.ShouldBeTrue();
        person.IsAdministrative.ShouldBeTrue();
    }

    [Fact]
    public void Assigning_a_new_default_Should_clear_the_previous_default_for_that_contact()
    {
        var contactId = Guid.CreateVersion7();
        var formerDefault = ContactPerson.Create(contactId, "Alex Rivera", null, null);
        var newDefault = ContactPerson.Create(contactId, "Bea Costa", null, null);
        var people = new[] { formerDefault, newDefault };
        ContactPerson.AssignDesignations(
            formerDefault,
            people,
            isDefault: true,
            isAdministrative: false
        );

        ContactPerson.AssignDesignations(
            newDefault,
            people,
            isDefault: true,
            isAdministrative: false
        );

        formerDefault.IsDefault.ShouldBeFalse();
        newDefault.IsDefault.ShouldBeTrue();
        people.Count(person => person.IsDefault).ShouldBe(1);
    }

    [Fact]
    public void Assigning_a_second_administrative_person_Should_preserve_the_existing_designation()
    {
        var contactId = Guid.CreateVersion7();
        var formerAdmin = ContactPerson.Create(contactId, "Alex Rivera", null, null);
        var newAdmin = ContactPerson.Create(contactId, "Bea Costa", null, null);
        var people = new[] { formerAdmin, newAdmin };
        ContactPerson.AssignDesignations(
            formerAdmin,
            people,
            isDefault: false,
            isAdministrative: true
        );

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            ContactPerson.AssignDesignations(
                newAdmin,
                people,
                isDefault: false,
                isAdministrative: true
            )
        );

        error.Code.ShouldBe("contacts.contact-person-administrative-already-assigned");
        formerAdmin.IsAdministrative.ShouldBeTrue();
        newAdmin.IsAdministrative.ShouldBeFalse();
        people.Count(person => person.IsAdministrative).ShouldBe(1);
    }

    [Fact]
    public void Contact_persons_Should_not_be_moved_to_another_contact()
    {
        var contactId = Guid.CreateVersion7();
        var person = ContactPerson.Create(contactId, "Alex Rivera", null, null);

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            person.Update(Guid.CreateVersion7(), "Alex Rivera", null, null)
        );

        error.Code.ShouldBe("contacts.contact-person-parent-immutable");
        person.ContactId.ShouldBe(contactId);
    }
}
