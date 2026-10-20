using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Projects;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Crew)]
public sealed class ProjectCancellationJourney_Should_PreserveInvoiceHistory(
    DistributedAppFixture app
) : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_WarnAboutOpenTasksAndKeepIssuedInvoiceAfterCancellation()
    {
        await RunJourneyAsync(
            "project-cancellation-invoice-history",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                var projectUrl = page.Url;

                await FinalizeDraftAsync(page);
                var invoiceUrl = page.Url;
                var projectId = new Uri(projectUrl).Segments[^1];
                await CreateOpenTaskAsync(page, projectId);

                await page.GotoAsync(projectUrl);
                await page.GetByRole(AriaRole.Button, new() { Name = "Cancel project" })
                    .ClickAsync();
                var previewWarning = page.GetByRole(AriaRole.Dialog).GetByRole(AriaRole.Alert);
                await previewWarning.WaitForAsync();
                (await previewWarning.InnerTextAsync()).ShouldBe(
                    "There are 1 open tasks, 0 assigned crew and 0 assigned vehicles. Cancellation will leave a persistent warning."
                );
                await page.GetByRole(AriaRole.Dialog)
                    .GetByRole(AriaRole.Button, new() { Name = "Confirm cancellation" })
                    .ClickAsync();
                await page.GetByRole(AriaRole.Dialog)
                    .WaitForAsync(new() { State = WaitForSelectorState.Hidden });

                await AssertCancellationWarningAsync(page);
                await page.ReloadAsync();
                await AssertCancellationWarningAsync(page);
                await page.GetByRole(AriaRole.Row)
                    .Filter(new() { HasText = "Camera kit with lenses" })
                    .GetByText("Invoiced")
                    .WaitForAsync();

                await page.GotoAsync(invoiceUrl);
                await page.GetByText("Issued and unalterable.").WaitForAsync();
                (await page.GetByText("Camera kit with lenses").CountAsync()).ShouldBeGreaterThan(
                    0
                );
            }
        );
    }

    [Fact]
    public async Task Journey_ShouldCompleteSelectedOpenTasksDuringCancellation()
    {
        await RunJourneyAsync(
            "project-cancellation-complete-tasks",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                var projectUrl = page.Url;
                var projectId = new Uri(projectUrl).Segments[^1];
                var completedStatus = await page.APIRequest.PostAsync(
                    "/api/project-coordination/statuses",
                    JsonOptions(
                        new
                        {
                            name = $"Completed E2E {projectId}",
                            position = 1,
                            completesProject = true,
                        }
                    )
                );
                completedStatus.Status.ShouldBe(201);
                using var completedStatusJson = JsonDocument.Parse(
                    await completedStatus.TextAsync()
                );
                var completedStatusId = completedStatusJson.RootElement.GetProperty("id").GetGuid();
                var openStatusId = await CreateOpenTaskAsync(page, projectId);

                await page.GotoAsync(projectUrl);
                await page.GetByRole(AriaRole.Button, new() { Name = "Cancel project" })
                    .ClickAsync();
                await page.GetByRole(AriaRole.Checkbox, new() { Name = "Complete 1 open tasks" })
                    .CheckAsync();
                await page.GetByRole(AriaRole.Dialog)
                    .GetByRole(AriaRole.Button, new() { Name = "Confirm cancellation" })
                    .ClickAsync();
                await page.GetByRole(AriaRole.Dialog)
                    .WaitForAsync(new() { State = WaitForSelectorState.Hidden });

                var boardResponse = await page.APIRequest.GetAsync(
                    $"/api/project-coordination/projects/{projectId}/board"
                );
                boardResponse.Status.ShouldBe(200);
                using var boardJson = JsonDocument.Parse(await boardResponse.TextAsync());
                var taskStatusId = boardJson
                    .RootElement.GetProperty("tasks")[0]
                    .GetProperty("statusId")
                    .GetGuid();
                taskStatusId.ShouldBe(completedStatusId);
                taskStatusId.ShouldNotBe(openStatusId);
                (await page.GetByRole(AriaRole.Alert).CountAsync()).ShouldBe(0);
            }
        );
    }

    [Fact]
    public async Task Journey_ShouldRemoveSelectedCrewTransportAndInvitations()
    {
        await RunJourneyAsync(
            "project-cancellation-crew-actions",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                var projectId = new Uri(page.Url).Segments[^1];
                var planning = await CreateCancellationPlanningAsync(page, projectId);

                var projectUrl = page.Url;
                await page.ReloadAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = "Cancel project" })
                    .ClickAsync();
                var dialog = page.GetByRole(AriaRole.Dialog);
                await dialog
                    .GetByRole(
                        AriaRole.Checkbox,
                        new() { Name = "Remove 1 crew members and 1 vehicles from planning" }
                    )
                    .CheckAsync();
                await dialog
                    .GetByRole(AriaRole.Checkbox, new() { Name = "Withdraw 1 active invitations" })
                    .CheckAsync();
                await dialog
                    .GetByRole(AriaRole.Button, new() { Name = "Confirm cancellation" })
                    .ClickAsync();
                await dialog.WaitForAsync(new() { State = WaitForSelectorState.Hidden });

                foreach (
                    var functionId in new[]
                    {
                        planning.CrewFunctionId,
                        planning.TransportFunctionId,
                    }
                )
                {
                    var functions = await page.APIRequest.GetAsync(
                        $"/api/projects/{projectId}/functions"
                    );
                    functions.Status.ShouldBe(200);
                    using var functionsJson = JsonDocument.Parse(await functions.TextAsync());
                    var function = functionsJson
                        .RootElement.EnumerateArray()
                        .First(item => item.GetProperty("id").GetGuid() == functionId);
                    function.GetProperty("crew").GetArrayLength().ShouldBe(0);
                    function.GetProperty("vehicles").GetArrayLength().ShouldBe(0);
                }
                var preview = await page.APIRequest.GetAsync(
                    $"/api/projects/{projectId}/cancellation-preview"
                );
                using var previewJson = JsonDocument.Parse(await preview.TextAsync());
                previewJson.RootElement.GetProperty("activeInvitationCount").GetInt32().ShouldBe(0);
                previewJson.RootElement.GetProperty("plannedCrewCount").GetInt32().ShouldBe(0);
                previewJson.RootElement.GetProperty("plannedTransportCount").GetInt32().ShouldBe(0);
                (await page.GetByRole(AriaRole.Alert).CountAsync()).ShouldBe(0);
                await page.GotoAsync(projectUrl);
            }
        );
    }

    [Fact]
    public async Task Journey_Should_LeaveUnselectedCancellationActionsUntouched()
    {
        await RunJourneyAsync(
            "project-cancellation-selected-actions",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                var projectId = new Uri(page.Url).Segments[^1];
                await CreateCancellationPlanningAsync(page, projectId);

                await page.ReloadAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = "Cancel project" })
                    .ClickAsync();
                var dialog = page.GetByRole(AriaRole.Dialog);
                var removeResources = dialog.GetByRole(
                    AriaRole.Checkbox,
                    new() { Name = "Remove 1 crew members and 1 vehicles from planning" }
                );
                await removeResources.CheckAsync();
                var withdrawInvitations = dialog.GetByRole(
                    AriaRole.Checkbox,
                    new() { Name = "Withdraw 1 active invitations" }
                );
                (await withdrawInvitations.IsCheckedAsync()).ShouldBeFalse();
                await dialog
                    .GetByRole(AriaRole.Button, new() { Name = "Confirm cancellation" })
                    .ClickAsync();
                await dialog.WaitForAsync(new() { State = WaitForSelectorState.Hidden });

                var preview = await page.APIRequest.GetAsync(
                    $"/api/projects/{projectId}/cancellation-preview"
                );
                preview.Status.ShouldBe(200);
                using var previewJson = JsonDocument.Parse(await preview.TextAsync());
                previewJson.RootElement.GetProperty("activeInvitationCount").GetInt32().ShouldBe(1);
                previewJson.RootElement.GetProperty("plannedCrewCount").GetInt32().ShouldBe(0);
                previewJson.RootElement.GetProperty("plannedTransportCount").GetInt32().ShouldBe(0);
                (await page.GetByRole(AriaRole.Alert).CountAsync()).ShouldBe(1);
            }
        );
    }

    private static async Task<Guid> CreateOpenTaskAsync(IPage page, string projectId)
    {
        var listedStatuses = await page.APIRequest.GetAsync("/api/project-coordination/statuses");
        listedStatuses.Status.ShouldBe(200);
        using var listedStatusesJson = JsonDocument.Parse(await listedStatuses.TextAsync());
        var existingOpenStatus = listedStatusesJson
            .RootElement.EnumerateArray()
            .FirstOrDefault(status => !status.GetProperty("completesProject").GetBoolean());
        var openStatusId =
            existingOpenStatus.ValueKind == JsonValueKind.Object
                ? existingOpenStatus.GetProperty("id").GetGuid()
                : await CreateOpenStatusAsync(page);
        var taskResponse = await page.APIRequest.PostAsync(
            "/api/project-coordination/tasks",
            JsonOptions(
                new
                {
                    projectId,
                    title = "Confirm location access",
                    description = (string?)null,
                    statusId = openStatusId,
                    position = 1,
                    assigneeId = (Guid?)null,
                }
            )
        );
        taskResponse.Status.ShouldBe(201);
        return openStatusId;
    }

    private static async Task<Guid> CreateOpenStatusAsync(IPage page)
    {
        var statusResponse = await page.APIRequest.PostAsync(
            "/api/project-coordination/statuses",
            JsonOptions(
                new
                {
                    name = $"Open E2E {Guid.NewGuid():N}",
                    position = 99,
                    completesProject = false,
                }
            )
        );
        statusResponse.Status.ShouldBe(201);
        using var statusJson = JsonDocument.Parse(await statusResponse.TextAsync());
        return statusJson.RootElement.GetProperty("id").GetGuid();
    }

    private static async Task<Guid> CreateFunctionAsync(
        IPage page,
        string projectId,
        string name,
        int kind
    )
    {
        var response = await page.APIRequest.PostAsync(
            $"/api/projects/{projectId}/functions",
            JsonOptions(
                new
                {
                    name,
                    kind,
                    startDate = "2026-09-14",
                    endDate = "2026-09-18",
                    requiredResources = 1,
                }
            )
        );
        response.Status.ShouldBe(201);
        using var json = JsonDocument.Parse(await response.TextAsync());
        return json.RootElement.GetProperty("id").GetGuid();
    }

    private async Task<CancellationPlanning> CreateCancellationPlanningAsync(
        IPage page,
        string projectId
    )
    {
        var resources = await CreateCancellationResourcesAsync(page);
        var crewFunctionId = await CreateFunctionAsync(page, projectId, "Crew", 0);
        var crewAssignment = await page.APIRequest.PostAsync(
            $"/api/projects/functions/{crewFunctionId}/crew",
            JsonOptions(new { crewMemberId = resources.Crew.Id, rateId = resources.Rate.Id })
        );
        crewAssignment.Status.ShouldBe(200);
        var transportFunctionId = await CreateFunctionAsync(page, projectId, "Transport", 1);
        var vehicleAssignment = await page.APIRequest.PostAsync(
            $"/api/projects/functions/{transportFunctionId}/vehicles",
            JsonOptions(new { vehicleId = resources.Vehicle.Id })
        );
        vehicleAssignment.Status.ShouldBe(200);
        var invitation = await page.APIRequest.PostAsync(
            $"/api/projects/functions/{crewFunctionId}/invitations",
            JsonOptions(
                new { crewMemberId = resources.Invitee.Id, remark = "Confirm availability" }
            )
        );
        invitation.Status.ShouldBe(201);
        return new(crewFunctionId, transportFunctionId);
    }

    private async Task<CancellationResources> CreateCancellationResourcesAsync(IPage page)
    {
        var crew = await CreateAsync(
            page,
            "/api/crew/members",
            new
            {
                firstName = $"Assigned {RunStamp}",
                role = "Camera operator",
                email = $"assigned-{RunStamp}@example.com",
            }
        );
        var invitee = await CreateAsync(
            page,
            "/api/crew/members",
            new
            {
                firstName = $"Invited {RunStamp}",
                role = "Assistant",
                email = $"invited-{RunStamp}@example.com",
            }
        );
        var vehicle = await CreateAsync(
            page,
            "/api/crew/vehicles",
            new { registrationNumber = $"CANCEL-{RunStamp}", seats = 4 }
        );
        var rate = await CreateAsync(
            page,
            "/api/crew/rates",
            new
            {
                name = $"Cancellation rate {RunStamp}",
                kind = 1,
                bands = new[]
                {
                    new
                    {
                        fromHour = 0,
                        fixedAmount = 100,
                        hourlyAmount = 20,
                    },
                },
                isDefault = true,
            }
        );
        return new(crew, invitee, vehicle, rate);
    }

    private static async Task<IdResponse> CreateAsync(IPage page, string route, object data)
    {
        var response = await page.APIRequest.PostAsync(route, JsonOptions(data));
        response.Status.ShouldBe(201);
        using var json = JsonDocument.Parse(await response.TextAsync());
        return new IdResponse(json.RootElement.GetProperty("id").GetGuid());
    }

    private static async Task AssertCancellationWarningAsync(IPage page)
    {
        var warning = page.GetByRole(AriaRole.Alert);
        await warning.WaitForAsync();
        (await warning.InnerTextAsync()).ShouldBe(
            "This project was cancelled with open operational work."
        );
    }

    private static APIRequestContextOptions JsonOptions<T>(T value) => new() { DataObject = value };

    private sealed record IdResponse(Guid Id);

    private sealed record CancellationPlanning(Guid CrewFunctionId, Guid TransportFunctionId);

    private sealed record CancellationResources(
        IdResponse Crew,
        IdResponse Invitee,
        IdResponse Vehicle,
        IdResponse Rate
    );
}
