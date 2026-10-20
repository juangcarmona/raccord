using System.Net;
using System.Net.Http.Json;
using Raccord.Contracts.Contacts;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class ContactAuthorizationTests(PostgresContainer postgres)
{
    [Fact]
    public async Task Contacts_Should_RequireAnAuthenticatedUser()
    {
        using var api = await RaccordApi.StartWithoutAuthBypassAsync(postgres);
        using var client = api.CreateClient();

        (await client.GetAsync("/api/contacts")).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Contacts_Should_RequireDirectoryAndMutationPermissions()
    {
        using var api = await RaccordApi.StartAsync(postgres);
        using var client = api.CreateClient();
        client.DefaultRequestHeaders.Add("X-Test-NoPermissions", "true");

        (await client.GetAsync("/api/contacts")).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        (
            await client.PostAsJsonAsync(
                "/api/contacts",
                new SaveContactRequest(
                    "Company",
                    "No permission",
                    null,
                    null,
                    null,
                    null,
                    ["Customer"]
                )
            )
        ).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Contacts_Should_RequirePermissionToEditAndLeaveContactUnchanged()
    {
        using var api = await RaccordApi.StartAsync(postgres);
        using var authorized = api.CreateClient();
        using var unauthorized = api.CreateClient();
        unauthorized.DefaultRequestHeaders.Add("X-Test-NoPermissions", "true");

        var created = await authorized.PostAsJsonAsync(
            "/api/contacts",
            new SaveContactRequest(
                "Company",
                "Authorized Contact",
                "CONTACT-100",
                null,
                null,
                null,
                ["Customer"]
            )
        );
        created.EnsureSuccessStatusCode();
        var contact = (await created.Content.ReadFromJsonAsync<ContactResponse>())!;

        var update = await unauthorized.PutAsJsonAsync(
            $"/api/contacts/{contact.Id}",
            new SaveContactRequest(
                "Company",
                "Unauthorized Change",
                "CONTACT-200",
                null,
                null,
                null,
                ["Supplier"]
            )
        );

        update.StatusCode.ShouldBe(System.Net.HttpStatusCode.Forbidden);
        var unchanged = await authorized.GetFromJsonAsync<ContactResponse>(
            $"/api/contacts/{contact.Id}"
        );
        unchanged!.LegalName.ShouldBe("Authorized Contact");
        unchanged.ContactNumber.ShouldBe("CONTACT-100");
        unchanged.Roles.ShouldBe(["Customer"]);
    }
}
