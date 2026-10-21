using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Raccord.Contracts.Projects;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class ProjectNumbersEndpointTests(PostgresContainer postgres) : IAsyncLifetime
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
    public async Task Creating_projects_Should_AssignSequentialProjectNumbers()
    {
        var first = await RaccordApi.ArrangeProjectAsync(client);
        var second = await CreateProjectAsync(first.CustomerId);

        (await ReadProjectAsync(first.Id))
            .GetProperty("projectNumber")
            .GetString()
            .ShouldBe("10001");
        second.GetProperty("projectNumber").GetString().ShouldBe("10002");
    }

    [Fact]
    public async Task Custom_project_numbers_Should_BePreservedAndSkippedByAutomaticAllocation()
    {
        var customer = await RaccordApi.ArrangeProjectAsync(client);
        var custom = await CreateProjectAsync(customer.CustomerId, "P-ALPHA");
        var automatic = await CreateProjectAsync(customer.CustomerId);

        custom.GetProperty("projectNumber").GetString().ShouldBe("P-ALPHA");
        automatic.GetProperty("projectNumber").GetString().ShouldBe("10002");
    }

    [Fact]
    public async Task Editing_project_number_Should_AllowCustomAndAutomaticValuesAndPreserveOmittedValue()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client);

        var custom = await UpdateProjectAsync(project.Id, project.CustomerId, "P-CUSTOM");
        custom.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await ReadProjectAsync(project.Id))
            .GetProperty("projectNumber")
            .GetString()
            .ShouldBe("P-CUSTOM");

        var legacy = await client.PutAsJsonAsync(
            $"/api/projects/{project.Id}",
            new UpdateProjectDetailsRequest(
                project.CustomerId,
                project.Name,
                project.StartDate,
                project.EndDate
            )
        );
        legacy.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await ReadProjectAsync(project.Id))
            .GetProperty("projectNumber")
            .GetString()
            .ShouldBe("P-CUSTOM");

        var cleared = await UpdateProjectAsync(project.Id, project.CustomerId, "");
        cleared.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await ReadProjectAsync(project.Id))
            .GetProperty("projectNumber")
            .GetString()
            .ShouldBe("10002");
    }

    [Fact]
    public async Task Assigning_duplicate_project_number_Should_RejectWithoutMutation()
    {
        var first = await RaccordApi.ArrangeProjectAsync(client);
        var second = await CreateProjectAsync(first.CustomerId, "P-SHARED");
        var secondId = Guid.Parse(second.GetProperty("id").GetString()!);

        var duplicate = await UpdateProjectAsync(first.Id, first.CustomerId, "P-SHARED");

        duplicate.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        (await ReadProjectAsync(first.Id))
            .GetProperty("projectNumber")
            .GetString()
            .ShouldBe("10001");
        (await ReadProjectAsync(secondId))
            .GetProperty("projectNumber")
            .GetString()
            .ShouldBe("P-SHARED");
    }

    [Fact]
    public async Task Concurrent_project_creation_Should_AssignUniqueNumbers()
    {
        var arrangement = await RaccordApi.ArrangeProjectAsync(client);
        var creates = Enumerable
            .Range(0, 8)
            .Select(_ => CreateProjectAsync(arrangement.CustomerId))
            .ToArray();

        var projects = await Task.WhenAll(creates);
        var numbers = projects
            .Select(project => project.GetProperty("projectNumber").GetString())
            .ToArray();

        numbers.Distinct(StringComparer.Ordinal).Count().ShouldBe(numbers.Length);
    }

    private async Task<JsonElement> CreateProjectAsync(
        Guid customerId,
        string? projectNumber = null
    )
    {
        var response = await client.PostAsJsonAsync(
            "/api/projects",
            new
            {
                name = "Numbered project",
                customerId,
                startDate = new DateOnly(2026, 5, 4),
                endDate = new DateOnly(2026, 5, 8),
                projectNumber,
            }
        );
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return document.RootElement.Clone();
    }

    private async Task<HttpResponseMessage> UpdateProjectAsync(
        Guid projectId,
        Guid customerId,
        string projectNumber
    ) =>
        await client.PutAsJsonAsync(
            $"/api/projects/{projectId}",
            new
            {
                customerId,
                name = "Numbered project",
                startDate = new DateOnly(2026, 5, 4),
                endDate = new DateOnly(2026, 5, 8),
                projectNumber,
            }
        );

    private async Task<JsonElement> ReadProjectAsync(Guid projectId)
    {
        using var document = JsonDocument.Parse(
            await client.GetStringAsync($"/api/projects/{projectId}")
        );
        return document.RootElement.Clone();
    }
}
