using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Raccord.Contracts.Projects;
using Raccord.Infrastructure.Persistence;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class ProjectAccountManagerEndpointTests(PostgresContainer postgres) : IAsyncLifetime
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
    public async Task Account_managers_Should_ListOnlyAdminAndOfficeUsers()
    {
        await CreateAdministratorAsync();
        var office = await CreateUserAsync("Office", "Office manager");
        var crew = await CreateUserAsync("Crew", "Crew member");
        var candidates = await client.GetFromJsonAsync<JsonElement>(
            "/api/projects/account-managers"
        );
        candidates
            .EnumerateArray()
            .Select(user => user.GetProperty("id").GetString())
            .ShouldContain(office.ToString());
        candidates
            .EnumerateArray()
            .Select(user => user.GetProperty("id").GetString())
            .ShouldNotContain(crew.ToString());
    }

    [Fact]
    public async Task Project_creation_Should_PersistAnEligibleAccountManager()
    {
        await CreateAdministratorAsync();
        var office = await CreateUserAsync("Office", "Office manager");
        var project = await RaccordApi.ArrangeProjectAsync(client);

        await CreateProjectWithManagerAsync(
            project.CustomerId,
            project.StartDate,
            project.EndDate,
            office
        );
    }

    [Fact]
    public async Task Project_update_Should_RejectAnIneligibleAccountManagerWithoutMutation()
    {
        await CreateAdministratorAsync();
        var office = await CreateUserAsync("Office", "Office manager");
        var crew = await CreateUserAsync("Crew", "Crew member");
        var project = await RaccordApi.ArrangeProjectAsync(client);
        var assigned = await client.PutAsJsonAsync(
            $"/api/projects/{project.Id}",
            new
            {
                customerId = project.CustomerId,
                name = project.Name,
                startDate = project.StartDate,
                endDate = project.EndDate,
                accountManagerId = office,
            }
        );
        assigned.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await ReadProjectAsync(project.Id))
            .GetProperty("accountManagerId")
            .GetString()
            .ShouldBe(office.ToString());

        var invalid = await client.PutAsJsonAsync(
            $"/api/projects/{project.Id}",
            new
            {
                customerId = project.CustomerId,
                name = project.Name,
                startDate = project.StartDate,
                endDate = project.EndDate,
                accountManagerId = crew,
            }
        );
        invalid.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        (await ReadProjectAsync(project.Id))
            .GetProperty("accountManagerId")
            .GetString()
            .ShouldBe(office.ToString());
    }

    private async Task CreateAdministratorAsync()
    {
        await using var scope = api.Services.CreateAsyncScope();
        (
            await scope
                .ServiceProvider.GetRequiredService<InstallationSetup>()
                .CreateFirstAdministratorAsync(
                    $"admin-{Guid.NewGuid():N}@example.test",
                    "ProjectAccountManager123!",
                    CancellationToken.None
                )
        ).Kind.ShouldBe(InstallationSetup.SetupResultKind.Created);
    }

    private async Task<Guid> CreateUserAsync(string role, string displayName)
    {
        await using var scope = api.Services.CreateAsyncScope();
        var users = scope.ServiceProvider.GetRequiredService<UserManager<RaccordUser>>();
        var email = $"{role.ToLowerInvariant()}-{Guid.NewGuid():N}@example.test";
        var user = new RaccordUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            DisplayName = displayName,
        };
        (await users.CreateAsync(user, "ProjectAccountManager123!")).Succeeded.ShouldBeTrue();
        (await users.AddToRoleAsync(user, role)).Succeeded.ShouldBeTrue();
        return user.Id;
    }

    private async Task<JsonElement> ReadProjectAsync(Guid projectId)
    {
        using var document = JsonDocument.Parse(
            await client.GetStringAsync($"/api/projects/{projectId}")
        );
        return document.RootElement.Clone();
    }

    private async Task CreateProjectWithManagerAsync(
        Guid customerId,
        DateOnly startDate,
        DateOnly endDate,
        Guid accountManagerId
    )
    {
        var response = await client.PostAsJsonAsync(
            "/api/projects",
            new
            {
                name = "Managed project",
                customerId,
                startDate,
                endDate,
                accountManagerId,
            }
        );
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        document
            .RootElement.GetProperty("accountManagerId")
            .GetString()
            .ShouldBe(accountManagerId.ToString());
    }
}
