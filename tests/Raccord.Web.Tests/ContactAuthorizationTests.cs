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
}
