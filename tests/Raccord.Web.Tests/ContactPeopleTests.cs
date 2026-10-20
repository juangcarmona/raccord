using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Raccord.Contracts.Contacts;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class ContactPeopleTests(PostgresContainer postgres) : IAsyncLifetime
{
    private RaccordApi api = null!;
    private HttpClient client = null!;

    public async ValueTask InitializeAsync()
    {
        api = await RaccordApi.StartAsync(postgres);
        client = api.CreateClient();
    }

    public async ValueTask DisposeAsync()
    {
        client.Dispose();
        await api.DisposeAsync();
    }

    [Fact]
    public async Task Contact_people_Should_CreateReadEditAndPreserveDesignationInvariants()
    {
        var contact = await CreateContactAsync("Production company");
        var formerDefault = await CreatePersonAsync(
            contact.Id,
            new SaveContactPersonRequest(
                "Alex Rivera",
                "alex@example.test",
                "+34 600 123 456",
                IsDefault: true,
                IsAdministrative: true
            )
        );
        var newDefault = await CreatePersonAsync(
            contact.Id,
            new SaveContactPersonRequest("Bea Costa", null, null)
        );

        var updatedResponse = await client.PutAsJsonAsync(
            PersonPath(contact.Id, newDefault.Id),
            new SaveContactPersonRequest(
                "Bea Costa",
                "bea@example.test",
                "600 100 200",
                IsDefault: true,
                IsAdministrative: false
            )
        );
        updatedResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

        var people = await client.GetFromJsonAsync<IReadOnlyList<ContactPersonResponse>>(
            PeoplePath(contact.Id)
        );
        people!.Count.ShouldBe(2);
        people.Single(person => person.Id == formerDefault.Id).IsDefault.ShouldBeFalse();
        people.Single(person => person.Id == newDefault.Id).IsDefault.ShouldBeTrue();
        people.Single(person => person.Id == formerDefault.Id).IsAdministrative.ShouldBeTrue();
        people.Single(person => person.Id == newDefault.Id).Email.ShouldBe("bea@example.test");

        var conflictingAdmin = await client.PutAsJsonAsync(
            PersonPath(contact.Id, newDefault.Id),
            new SaveContactPersonRequest(
                "Bea Costa",
                "bea@example.test",
                "600 100 200",
                IsDefault: true,
                IsAdministrative: true
            )
        );
        conflictingAdmin.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var problem = await conflictingAdmin.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Extensions["code"]!
            .ToString()
            .ShouldBe("contacts.contact-person-administrative-already-assigned");

        var unchanged = await client.GetFromJsonAsync<ContactPersonResponse>(
            PersonPath(contact.Id, newDefault.Id)
        );
        unchanged!.IsDefault.ShouldBeTrue();
        unchanged.IsAdministrative.ShouldBeFalse();
    }

    [Fact]
    public async Task Contact_people_Should_RejectInvalidEmailWithoutSavingThePerson()
    {
        var contact = await CreateContactAsync("Invalid contact person company");

        var response = await client.PostAsJsonAsync(
            PeoplePath(contact.Id),
            new SaveContactPersonRequest("Alex Rivera", "not-an-email", null)
        );

        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Extensions["code"]!.ToString().ShouldBe("contacts.contact-person-email-invalid");
        var people = await client.GetFromJsonAsync<IReadOnlyList<ContactPersonResponse>>(
            PeoplePath(contact.Id)
        );
        people!.ShouldBeEmpty();
    }

    [Fact]
    public async Task Contact_people_Should_ReturnNotFoundForMissingParentsPeopleAndMovedPeople()
    {
        var contact = await CreateContactAsync("First contact");
        var otherContact = await CreateContactAsync("Second contact");
        var person = await CreatePersonAsync(
            contact.Id,
            new SaveContactPersonRequest("Alex Rivera", null, null)
        );

        (await client.GetAsync(PeoplePath(Guid.CreateVersion7()))).StatusCode.ShouldBe(
            HttpStatusCode.NotFound
        );
        (await client.GetAsync(PersonPath(contact.Id, Guid.CreateVersion7()))).StatusCode.ShouldBe(
            HttpStatusCode.NotFound
        );
        (
            await client.PutAsJsonAsync(
                PersonPath(otherContact.Id, person.Id),
                new SaveContactPersonRequest("Moved Alex", null, null)
            )
        ).StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var unchanged = await client.GetFromJsonAsync<ContactPersonResponse>(
            PersonPath(contact.Id, person.Id)
        );
        unchanged!.ContactId.ShouldBe(contact.Id);
        unchanged.Name.ShouldBe("Alex Rivera");
    }

    [Fact]
    public async Task Concurrent_contact_person_default_changes_Should_LeaveOneDefaultPerContact()
    {
        var contact = await CreateContactAsync("Concurrent contact");
        var first = await CreatePersonAsync(
            contact.Id,
            new SaveContactPersonRequest("Alex Rivera", null, null, IsDefault: true)
        );
        var second = await CreatePersonAsync(
            contact.Id,
            new SaveContactPersonRequest("Bea Costa", null, null)
        );

        var updates = await Task.WhenAll(
            client.PutAsJsonAsync(
                PersonPath(contact.Id, first.Id),
                new SaveContactPersonRequest("Alex Rivera", null, null, IsDefault: true)
            ),
            client.PutAsJsonAsync(
                PersonPath(contact.Id, second.Id),
                new SaveContactPersonRequest("Bea Costa", null, null, IsDefault: true)
            )
        );

        var statuses = updates.Select(response => response.StatusCode).ToArray();
        statuses.ShouldAllBe(status =>
            status == HttpStatusCode.OK || status == HttpStatusCode.Conflict
        );
        statuses.ShouldContain(HttpStatusCode.OK);
        var people = await client.GetFromJsonAsync<IReadOnlyList<ContactPersonResponse>>(
            PeoplePath(contact.Id)
        );
        people!.Count(person => person.IsDefault).ShouldBe(1);
    }

    [Fact]
    public async Task Contact_people_Should_UseTheExistingContactsPermissions()
    {
        var contact = await CreateContactAsync("Authorization contact");
        client.DefaultRequestHeaders.Add("X-Test-NoPermissions", "true");

        (await client.GetAsync(PeoplePath(contact.Id))).StatusCode.ShouldBe(
            HttpStatusCode.Forbidden
        );
        (
            await client.PostAsJsonAsync(
                PeoplePath(contact.Id),
                new SaveContactPersonRequest("Alex Rivera", null, null)
            )
        ).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    private async Task<ContactResponse> CreateContactAsync(string name)
    {
        var response = await client.PostAsJsonAsync(
            "/api/contacts",
            new SaveContactRequest("Company", name, null, null, null, null, ["Customer"])
        );
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ContactResponse>())!;
    }

    private async Task<ContactPersonResponse> CreatePersonAsync(
        Guid contactId,
        SaveContactPersonRequest request
    )
    {
        var response = await client.PostAsJsonAsync(PeoplePath(contactId), request);
        response.StatusCode.ShouldBe(
            HttpStatusCode.Created,
            await response.Content.ReadAsStringAsync()
        );
        return (await response.Content.ReadFromJsonAsync<ContactPersonResponse>())!;
    }

    private static string PeoplePath(Guid contactId) => $"/api/contacts/{contactId}/people";

    private static string PersonPath(Guid contactId, Guid personId) =>
        $"/api/contacts/{contactId}/people/{personId}";
}
