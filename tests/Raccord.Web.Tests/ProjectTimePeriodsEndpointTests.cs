using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Raccord.Contracts.Crew;
using Raccord.Contracts.Projects;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class ProjectTimePeriodsEndpointTests(PostgresContainer postgres) : IAsyncLifetime
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
    public async Task Project_time_periods_Should_CreateEditDesignateDuplicateReorderAndPersist()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client);
        var initial = await GetPeriodsAsync(project.Id);
        initial.Count.ShouldBe(2);
        var added = await AddPeriodAsync(project.Id, "Load-in");
        var start = new DateTimeOffset(2026, 5, 4, 8, 0, 0, TimeSpan.Zero);
        var end = start.AddHours(10);
        var updated = await UpdatePeriodAsync(project.Id, added.Id, "Build", start, end);
        updated.Name.ShouldBe("Build");
        updated.Start.ShouldBe(start);
        updated.End.ShouldBe(end);
        updated.ShowOnExternalDocuments.ShouldBeFalse();
        var designated = await DesignatePeriodAsync(project.Id, added.Id);
        designated.IsDefaultPlanning.ShouldBeTrue();
        var duplicate = await DuplicatePeriodAsync(project.Id, added.Id);
        duplicate.Name.ShouldBe("Build copy");
        duplicate.IsDefaultPlanning.ShouldBeFalse();
        duplicate.ShowOnExternalDocuments.ShouldBeFalse();

        var ids = new[] { duplicate.Id, initial[0].Id, added.Id, initial[1].Id };
        await ReorderPeriodsAsync(project.Id, ids);
        var reloaded = await GetPeriodsAsync(project.Id);
        reloaded.Select(period => period.Id).ToArray().ShouldBe(ids);
        reloaded.Select(period => period.Position).ToArray().ShouldBe([0, 1, 2, 3]);
    }

    private async Task<List<ProjectTimePeriodResponse>> GetPeriodsAsync(Guid projectId) =>
        await client.GetFromJsonAsync<List<ProjectTimePeriodResponse>>(
            $"/api/projects/{projectId}/periods"
        ) ?? throw new InvalidOperationException("Project period response was empty.");

    private async Task<ProjectTimePeriodResponse> AddPeriodAsync(Guid projectId, string name)
    {
        var response = await client.PostAsJsonAsync(
            $"/api/projects/{projectId}/periods",
            new AddProjectTimePeriodRequest(name)
        );
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        return (await response.Content.ReadFromJsonAsync<ProjectTimePeriodResponse>())!;
    }

    private async Task<ProjectTimePeriodResponse> UpdatePeriodAsync(
        Guid projectId,
        Guid periodId,
        string name,
        DateTimeOffset start,
        DateTimeOffset end
    )
    {
        var response = await client.PutAsJsonAsync(
            $"/api/projects/{projectId}/periods/{periodId}",
            new UpdateProjectTimePeriodRequest(
                name,
                start,
                end,
                ShowOnInternalDocuments: true,
                ShowOnExternalDocuments: false
            )
        );
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        return (await response.Content.ReadFromJsonAsync<ProjectTimePeriodResponse>())!;
    }

    private async Task<ProjectTimePeriodResponse> DesignatePeriodAsync(
        Guid projectId,
        Guid periodId
    )
    {
        var response = await client.PutAsJsonAsync(
            $"/api/projects/{projectId}/periods/{periodId}/designation",
            new DesignateProjectTimePeriodRequest(true, false)
        );
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        return (await response.Content.ReadFromJsonAsync<ProjectTimePeriodResponse>())!;
    }

    private async Task<ProjectTimePeriodResponse> DuplicatePeriodAsync(
        Guid projectId,
        Guid periodId
    )
    {
        var response = await client.PostAsync(
            $"/api/projects/{projectId}/periods/{periodId}/duplicate",
            content: null
        );
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        return (await response.Content.ReadFromJsonAsync<ProjectTimePeriodResponse>())!;
    }

    private async Task ReorderPeriodsAsync(Guid projectId, IReadOnlyList<Guid> periodIds)
    {
        var response = await client.PutAsJsonAsync(
            $"/api/projects/{projectId}/periods/order",
            new ReorderProjectTimePeriodsRequest(periodIds)
        );
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Updating_a_usage_period_outside_planning_Should_BeRejectedWithoutMutation()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client);
        var periods = await GetPeriodsAsync(project.Id);
        var planning = periods.Single(period => period.IsDefaultPlanning);
        var usage = periods.Single(period => period.IsDefaultUsage);
        var start = new DateTimeOffset(2026, 5, 4, 8, 0, 0, TimeSpan.Zero);

        var planned = await client.PutAsJsonAsync(
            $"/api/projects/{project.Id}/periods/{planning.Id}",
            new UpdateProjectTimePeriodRequest(
                planning.Name,
                start,
                start.AddDays(2),
                planning.ShowOnInternalDocuments,
                planning.ShowOnExternalDocuments
            )
        );
        planned.StatusCode.ShouldBe(HttpStatusCode.OK);
        var rejected = await client.PutAsJsonAsync(
            $"/api/projects/{project.Id}/periods/{usage.Id}",
            new UpdateProjectTimePeriodRequest(
                usage.Name,
                start.AddDays(2),
                start.AddDays(3),
                usage.ShowOnInternalDocuments,
                usage.ShowOnExternalDocuments
            )
        );

        rejected.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var unchanged = await GetPeriodsAsync(project.Id);
        unchanged.Single(period => period.Id == usage.Id).Start.ShouldBeNull();
        unchanged.Single(period => period.Id == usage.Id).End.ShouldBeNull();
    }

    [Fact]
    public async Task Reordering_with_a_missing_period_Should_BeRejectedWithoutMutation()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client);
        var original = await GetPeriodsAsync(project.Id);
        var response = await client.PutAsJsonAsync(
            $"/api/projects/{project.Id}/periods/order",
            new ReorderProjectTimePeriodsRequest([original[0].Id, Guid.CreateVersion7()])
        );

        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var unchanged = await GetPeriodsAsync(project.Id);
        unchanged
            .Select(period => period.Id)
            .ToArray()
            .ShouldBe(original.Select(period => period.Id).ToArray());
        unchanged.Select(period => period.Position).ToArray().ShouldBe([0, 1]);
    }

    [Fact]
    public async Task Completing_a_project_Should_BlockOperationalChangesUntilReopened()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client);
        var taskStatusResponse = await client.PostAsJsonAsync(
            "/api/project-coordination/statuses",
            new CreateProjectTaskStatusRequest("Open", 0, CompletesProject: false)
        );
        taskStatusResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
        using var taskStatusBody = JsonDocument.Parse(
            await taskStatusResponse.Content.ReadAsStringAsync()
        );
        var taskStatusId = taskStatusBody.RootElement.GetProperty("id").GetGuid();
        var completed = await client.PutAsJsonAsync(
            $"/api/projects/{project.Id}/status",
            new ChangeProjectStatusRequest("complete")
        );
        completed.StatusCode.ShouldBe(HttpStatusCode.OK);

        await AssertOperationalChangesRejectedAsync(project, taskStatusId);

        var reopened = await client.PutAsJsonAsync(
            $"/api/projects/{project.Id}/status",
            new ChangeProjectStatusRequest("in-progress")
        );
        reopened.StatusCode.ShouldBe(HttpStatusCode.OK);
        var editablePeriod = await client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/periods",
            new AddProjectTimePeriodRequest("Show")
        );
        editablePeriod.StatusCode.ShouldBe(HttpStatusCode.Created);
        var editableFunction = await client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/functions",
            new CreateProjectFunctionRequest(
                "Camera operator",
                CrewFunctionKind.Crew,
                project.StartDate,
                project.EndDate
            )
        );
        editableFunction.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    private async Task AssertOperationalChangesRejectedAsync(
        ProjectResponse project,
        Guid taskStatusId
    )
    {
        var period = await client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/periods",
            new AddProjectTimePeriodRequest("Show")
        );
        period.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var function = await client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/functions",
            new CreateProjectFunctionRequest(
                "Camera operator",
                CrewFunctionKind.Crew,
                project.StartDate,
                project.EndDate
            )
        );
        function.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var task = await client.PostAsJsonAsync(
            "/api/project-coordination/tasks",
            new CreateProjectTaskRequest(
                project.Id,
                "Confirm location",
                null,
                taskStatusId,
                0,
                null
            )
        );
        task.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var schedule = await client.PostAsJsonAsync(
            "/api/project-coordination/schedules",
            new CreateProjectScheduleRequest(
                project.Id,
                "Load-in",
                project.StartDate,
                project.EndDate,
                null
            )
        );
        schedule.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var timeEntry = await client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/time",
            new RecordProjectTimeRequest(null, "Camera", 8m, 8m, "E2E")
        );
        timeEntry.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var projectNote = await client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/context/notes",
            new AddProjectNoteRequest("Update", "New note", "E2E")
        );
        projectNote.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var equipmentPlan = await client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/packing-lines",
            new PlanEquipmentRequest(Guid.CreateVersion7(), 1)
        );
        equipmentPlan.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}
