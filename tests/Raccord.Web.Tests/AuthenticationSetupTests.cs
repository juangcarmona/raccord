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

        var second = await client.PostAsJsonAsync(
            "/api/setup",
            new SetupRequest("second-admin@example.com", "SecondAdmin123!@#")
        );
        second.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        (await api.QueryScalarAsync<long>("select count(*) from workspace.users")).ShouldBe(1);
    }

    private sealed record SetupRequest(string Email, string Password);

    private sealed record SetupStatusResponse(bool Required);
}
