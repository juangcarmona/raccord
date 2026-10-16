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

        await Profile_Should_SavePersonalPreferencesAndRemainOwnedByTheCurrentAccount();

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

        var second = await client.PostAsJsonAsync(
            "/api/setup",
            new SetupRequest("second-admin@example.com", "SecondAdmin123!@#")
        );
        second.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        (await api.QueryScalarAsync<long>("select count(*) from workspace.users")).ShouldBe(1);
    }

    private async Task Profile_Should_SavePersonalPreferencesAndRemainOwnedByTheCurrentAccount()
    {
        var current = await client.GetFromJsonAsync<CurrentUserResponse>("/api/auth/me");
        current.ShouldNotBeNull();
        current.Email.ShouldBe("first-admin@example.com");
        current.PreferredLanguage.ShouldBe("es");
        current.CanManageRoles.ShouldBeTrue();

        var profile = await client.PutAsJsonAsync(
            "/api/auth/me",
            new
            {
                DisplayName = "First Admin",
                PreferredLanguage = "en",
                Email = "changed@example.com",
            }
        );
        profile.IsSuccessStatusCode.ShouldBeTrue(await profile.Content.ReadAsStringAsync());
        var updated = await profile.Content.ReadFromJsonAsync<CurrentUserResponse>();
        updated!.DisplayName.ShouldBe("First Admin");
        updated.PreferredLanguage.ShouldBe("en");

        var invalidProfile = await client.PutAsJsonAsync(
            "/api/auth/me",
            new UpdateProfileRequest(" ", "en")
        );
        invalidProfile.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var invalidLanguage = await client.PutAsJsonAsync(
            "/api/auth/me",
            new UpdateProfileRequest("First Admin", "fr")
        );
        invalidLanguage.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var restored = await client.GetFromJsonAsync<CurrentUserResponse>("/api/auth/me");
        restored!.Email.ShouldBe("first-admin@example.com");
        restored.DisplayName.ShouldBe("First Admin");
        restored.PreferredLanguage.ShouldBe("en");
    }

    private sealed record SetupRequest(string Email, string Password);

    private sealed record SetupStatusResponse(bool Required);

    private sealed record UpdateProfileRequest(string DisplayName, string PreferredLanguage);

    private sealed record CurrentUserResponse(
        string Email,
        string? DisplayName,
        string PreferredLanguage,
        bool CanManageRoles
    );

    private sealed record RoleResponse(
        string Name,
        bool Predefined,
        IReadOnlyList<string> Permissions,
        int UserCount
    );

    private sealed record RoleRequest(string Name, IReadOnlyCollection<string> Permissions);
}
