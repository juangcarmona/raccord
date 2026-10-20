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
    public async Task Generating_functions_from_selected_periods_Should_CreateOneLinkedFunctionPerKindAndPeriod()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client);
        var periods = await GetPeriodsAsync(project.Id);
        var first = await UpdatePeriodAsync(
            project.Id,
            periods[0].Id,
            "Planning",
            new DateTimeOffset(project.StartDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)),
            new DateTimeOffset(
                project.EndDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc).AddDays(1)
            )
        );
        var second = await AddPeriodAsync(project.Id, "Shoot");
        await UpdatePeriodAsync(
            project.Id,
            second.Id,
            "Shoot",
            new DateTimeOffset(
                project.StartDate.AddDays(2).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
            ),
            new DateTimeOffset(
                project
                    .EndDate.AddDays(2)
                    .ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
                    .AddDays(1)
            )
        );

        var response = await client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/functions/from-periods",
            new GenerateProjectFunctionsFromPeriodsRequest(
                [first.Id, second.Id],
                [CrewFunctionKind.Crew, CrewFunctionKind.Transport]
            )
        );

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var generated = await response.Content.ReadFromJsonAsync<List<ProjectFunctionResponse>>();
        generated.ShouldNotBeNull();
        generated!.Count.ShouldBe(4);
        generated.Select(function => function.ProjectTimePeriodId).Distinct().Count().ShouldBe(2);
        generated.Select(function => function.Kind).Distinct().Count().ShouldBe(2);
        generated.ShouldAllBe(function => function.RequiredResources == 1);
        var persisted = await client.GetFromJsonAsync<List<ProjectFunctionResponse>>(
            $"/api/projects/{project.Id}/functions"
        );
        persisted!
            .Select(function => function.Id)
            .ToHashSet()
            .SetEquals(generated.Select(function => function.Id))
            .ShouldBeTrue();
    }

    [Fact]
    public async Task Generating_functions_with_an_untimed_period_Should_RejectTheWholeBatch()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client);
        var periods = await GetPeriodsAsync(project.Id);
        var response = await client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/functions/from-periods",
            new GenerateProjectFunctionsFromPeriodsRequest(
                [periods[0].Id, periods[1].Id],
                [CrewFunctionKind.Crew, CrewFunctionKind.Transport]
            )
        );

        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var persisted = await client.GetFromJsonAsync<List<ProjectFunctionResponse>>(
            $"/api/projects/{project.Id}/functions"
        );
        persisted.ShouldNotBeNull();
        persisted!.ShouldBeEmpty();
    }

    [Fact]
    public async Task Updating_a_linked_period_Should_MoveFunctionsAndShiftsButRejectInvalidRangesAtomically()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client);
        var period = await CreateTimedPeriodAsync(project.Id);
        var linked = await GenerateLinkedCrewFunctionAsync(project.Id, period.Id);
        var unlinkedResponse = await client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/functions",
            new CreateProjectFunctionRequest(
                "Independent crew",
                CrewFunctionKind.Crew,
                linked.StartDate,
                linked.EndDate
            )
        );
        unlinkedResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
        var unlinked = (
            await unlinkedResponse.Content.ReadFromJsonAsync<ProjectFunctionResponse>()
        )!;
        var shiftDate = linked.StartDate.AddDays(2);
        var shiftResponse = await client.PostAsJsonAsync(
            $"/api/projects/functions/{linked.Id}/shifts",
            new CreateProjectShiftRequest(shiftDate, new TimeOnly(9, 0), new TimeOnly(17, 0))
        );
        shiftResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

        await AssertLinkedPeriodPropagationAsync(
            project.Id,
            period,
            linked.Id,
            unlinked,
            shiftDate
        );
    }

    private async Task AssertLinkedPeriodPropagationAsync(
        Guid projectId,
        ProjectTimePeriodResponse period,
        Guid linkedFunctionId,
        ProjectFunctionResponse unlinked,
        DateOnly shiftDate
    )
    {
        var movedStart = new DateTimeOffset(2026, 5, 5, 0, 0, 0, TimeSpan.Zero);
        var movedEnd = new DateTimeOffset(2026, 5, 9, 0, 0, 0, TimeSpan.Zero);
        var moved = await UpdatePeriodAsync(
            projectId,
            period.Id,
            period.Name,
            movedStart,
            movedEnd
        );
        moved.Start.ShouldBe(movedStart);
        var functions = await GetFunctionsAsync(projectId);
        var linked = functions.Single(function => function.Id == linkedFunctionId);
        linked.StartDate.ShouldBe(DateOnly.FromDateTime(movedStart.DateTime));
        linked.EndDate.ShouldBe(DateOnly.FromDateTime(movedEnd.DateTime));
        linked.Shifts.Single().Date.ShouldBe(shiftDate.AddDays(1));
        functions
            .Single(function => function.Id == unlinked.Id)
            .StartDate.ShouldBe(unlinked.StartDate);

        var rejected = await client.PutAsJsonAsync(
            $"/api/projects/{projectId}/periods/{period.Id}",
            new UpdateProjectTimePeriodRequest(
                period.Name,
                new DateTimeOffset(2026, 5, 6, 0, 0, 0, TimeSpan.Zero),
                new DateTimeOffset(2026, 5, 7, 0, 0, 0, TimeSpan.Zero),
                period.ShowOnInternalDocuments,
                period.ShowOnExternalDocuments
            )
        );
        rejected.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        (await GetPeriodsAsync(projectId))
            .Single(item => item.Id == period.Id)
            .Start.ShouldBe(movedStart);
        linked = (await GetFunctionsAsync(projectId)).Single(function =>
            function.Id == linkedFunctionId
        );
        linked.StartDate.ShouldBe(DateOnly.FromDateTime(movedStart.DateTime));
        linked.Shifts.Single().Date.ShouldBe(shiftDate.AddDays(1));
    }

    private async Task<ProjectTimePeriodResponse> CreateTimedPeriodAsync(Guid projectId)
    {
        var period = await AddPeriodAsync(projectId, "Shoot");
        var start = new DateTimeOffset(2026, 5, 4, 0, 0, 0, TimeSpan.Zero);
        return await UpdatePeriodAsync(projectId, period.Id, period.Name, start, start.AddDays(4));
    }

    private async Task<ProjectFunctionResponse> GenerateLinkedCrewFunctionAsync(
        Guid projectId,
        Guid periodId
    )
    {
        var response = await client.PostAsJsonAsync(
            $"/api/projects/{projectId}/functions/from-periods",
            new GenerateProjectFunctionsFromPeriodsRequest([periodId], [CrewFunctionKind.Crew])
        );
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var generated = (
            await response.Content.ReadFromJsonAsync<List<ProjectFunctionResponse>>()
        )!;
        return generated.Single();
    }

    private async Task<List<ProjectFunctionResponse>> GetFunctionsAsync(Guid projectId) =>
        await client.GetFromJsonAsync<List<ProjectFunctionResponse>>(
            $"/api/projects/{projectId}/functions"
        ) ?? throw new InvalidOperationException("Project function response was empty.");

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
        await AssertProjectPlanningRejectedAsync(project);
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

    private async Task AssertProjectPlanningRejectedAsync(ProjectResponse project)
    {
        var currentPeriods = await GetPeriodsAsync(project.Id);
        var generatedFunctions = await client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/functions/from-periods",
            new GenerateProjectFunctionsFromPeriodsRequest(
                [currentPeriods[0].Id],
                [CrewFunctionKind.Crew]
            )
        );
        generatedFunctions.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        (await GetFunctionsAsync(project.Id)).ShouldBeEmpty();
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
    }
}
