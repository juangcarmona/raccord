using System.Net;
using System.Net.Http.Json;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class AuthenticationSetupTests(PostgresContainer postgres) : IAsyncLifetime
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
    public async Task First_setup_Should_CreateAdminAndRefuseASecondSetup()
    {
        var before = await client.GetFromJsonAsync<SetupStatusResponse>("/api/setup/status");
        before!.Required.ShouldBeTrue();

        var created = await client.PostAsJsonAsync(
            "/api/setup",
            new SetupRequest("first-admin@example.com", "FirstAdmin123!@#")
        );
        created.IsSuccessStatusCode.ShouldBeTrue(await created.Content.ReadAsStringAsync());

        var after = await client.GetFromJsonAsync<SetupStatusResponse>("/api/setup/status");
        after!.Required.ShouldBeFalse();
        (await api.QueryScalarAsync<long>("select count(*) from workspace.users")).ShouldBe(1);
        (await api.QueryScalarAsync<long>("select count(*) from workspace.roles")).ShouldBe(4);
        (await api.QueryScalarAsync<long>("select count(*) from workspace.user_roles")).ShouldBe(1);

        var roles =
            await client.GetFromJsonAsync<List<RoleResponse>>("/api/roles")
            ?? throw new InvalidOperationException("The role list was empty.");
        roles.Select(role => role.Name).ShouldBe(["Admin", "Crew", "Office", "Warehouse"]);
        roles
            .Single(role => string.Equals(role.Name, "Admin", StringComparison.Ordinal))
            .Permissions.ShouldContain("projects:read");

        var custom = await client.PostAsJsonAsync(
            "/api/roles",
            new RoleRequest("Read-only", ["projects:read"])
        );
        custom.StatusCode.ShouldBe(HttpStatusCode.Created);

        var invitation = await client.PostAsJsonAsync(
            "/api/invitations",
            new InvitationRequest("crew@example.com", "Crew")
        );
        invitation.StatusCode.ShouldBe(HttpStatusCode.OK);
        var invitationDetails = (await invitation.Content.ReadFromJsonAsync<InvitationResponse>())!;
        var joined = await client.PostAsJsonAsync(
            $"/api/invitations/{invitationDetails.Token}/accept",
            new JoinRequest("CrewMember123!@#")
        );
        joined.StatusCode.ShouldBe(HttpStatusCode.OK);

        var replay = await client.PostAsJsonAsync(
            $"/api/invitations/{invitationDetails.Token}/accept",
            new JoinRequest("CrewMember123!@#")
        );
        replay.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        (await api.QueryScalarAsync<long>("select count(*) from workspace.users")).ShouldBe(2);
        (
            await api.QueryScalarAsync<long>(
                "select count(*) from workspace.user_roles ur join workspace.roles r on r.id = ur.role_id where r.name = 'Crew'"
            )
        ).ShouldBe(1);

        var second = await client.PostAsJsonAsync(
            "/api/setup",
            new SetupRequest("second-admin@example.com", "SecondAdmin123!@#")
        );
        second.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        (await api.QueryScalarAsync<long>("select count(*) from workspace.users")).ShouldBe(1);
    }

    private sealed record SetupRequest(string Email, string Password);

    private sealed record SetupStatusResponse(bool Required);

    private sealed record RoleResponse(
        string Name,
        bool Predefined,
        IReadOnlyList<string> Permissions,
        int UserCount
    );

    private sealed record RoleRequest(string Name, IReadOnlyCollection<string> Permissions);

    private sealed record InvitationRequest(string Email, string Role);

    private sealed record InvitationResponse(string Token, DateTimeOffset ExpiresAt);

    private sealed record JoinRequest(string Password);
}
