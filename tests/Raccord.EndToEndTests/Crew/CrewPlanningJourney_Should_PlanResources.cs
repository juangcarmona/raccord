using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Crew;

/// Given a project, crew and vehicle, when a coordinator creates functions, shifts and assignments, then staffing status and conflicts remain visible.
/// ProductShape: BC-CREW-TRANSPORT, JRN-CRW-STAFF-A-PROJECT, JRN-CRW-PLAN-INTERNAL-WORK, UC-CRW-ADD-FUNCTION-TO-PROJECT, UC-CRW-PLAN-RESOURCE-ON-FUNCTION, UC-CRW-CREATE-SHIFT, UC-CRW-PLAN-RESOURCE-ON-SHIFT, UC-CRW-RESOLVE-PLANNING-CONFLICTS.
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Crew)]
public sealed class CrewPlanningJourney_Should_PlanResources(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
#pragma warning disable MA0051, CA1861
    [Fact]
    public async Task Journey_Should_PlanCrewAndVehicleOnProjectFunctions()
    {
        await RunJourneyAsync(
            "crew-planning",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                var projectId = new Uri(page.Url).Segments[^1];
                var crew = await CreateAsync(
                    page,
                    "/api/crew/members",
                    new
                    {
                        firstName = $"Planner {RunStamp}",
                        role = "Producer",
                        email = $"planner-{RunStamp}@example.com",
                    }
                );
                var vehicle = await CreateAsync(
                    page,
                    "/api/crew/vehicles",
                    new { registrationNumber = $"PLAN-{RunStamp}", seats = 4 }
                );
                var rate = await CreateAsync(
                    page,
                    "/api/crew/rates",
                    new
                    {
                        name = $"Plan rate {RunStamp}",
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
                var functionResponse = await page.APIRequest.PostAsync(
                    $"/api/projects/{projectId}/functions",
                    new()
                    {
                        DataObject = new
                        {
                            name = "Camera crew",
                            kind = 0,
                            startDate = "2026-09-14",
                            endDate = "2026-09-18",
                            requiredResources = 1,
                        },
                    }
                );
                functionResponse.Status.ShouldBe((int)HttpStatusCode.Created);
                var function = (await functionResponse.JsonAsync<FunctionResponse>())!;
                var shift = await page.APIRequest.PostAsync(
                    $"/api/projects/functions/{function.Id}/shifts",
                    new()
                    {
                        DataObject = new
                        {
                            date = "2026-09-15",
                            startTime = "09:00:00",
                            endTime = "17:00:00",
                        },
                    }
                );
                shift.Status.ShouldBe((int)HttpStatusCode.OK);
                var assignment = await page.APIRequest.PostAsync(
                    $"/api/projects/functions/{function.Id}/crew",
                    new()
                    {
                        DataObject = new
                        {
                            crewMemberId = crew.Id,
                            rateId = rate.Id,
                            includeTravel = true,
                            isProjectLeader = true,
                            remark = "Call sheet lead",
                        },
                    }
                );
                assignment.Status.ShouldBe((int)HttpStatusCode.OK);
                var planned = (await assignment.JsonAsync<FunctionResponse>())!;
                planned.IsStaffed.ShouldBeTrue();
                planned.Shifts.ShouldContain(item => item.Date == "2026-09-15");
                planned.Crew.ShouldContain(item =>
                    item.CrewMemberId == crew.Id && item.RateId == rate.Id && item.IsProjectLeader
                );
                var transportResponse = await page.APIRequest.PostAsync(
                    $"/api/projects/{projectId}/functions",
                    new()
                    {
                        DataObject = new
                        {
                            name = "Transport",
                            kind = 1,
                            startDate = "2026-09-14",
                            endDate = "2026-09-18",
                            requiredResources = 1,
                        },
                    }
                );
                transportResponse.Status.ShouldBe((int)HttpStatusCode.Created);
                var transport = (await transportResponse.JsonAsync<FunctionResponse>())!;
                var vehicleAssignment = await page.APIRequest.PostAsync(
                    $"/api/projects/functions/{transport.Id}/vehicles",
                    new() { DataObject = new { vehicleId = vehicle.Id, remark = "Equipment van" } }
                );
                vehicleAssignment.Status.ShouldBe((int)HttpStatusCode.OK);
                ((await vehicleAssignment.JsonAsync<FunctionResponse>())!).IsStaffed.ShouldBeTrue();
                var listed = await page.APIRequest.GetAsync($"/api/projects/{projectId}/functions");
                listed.Status.ShouldBe((int)HttpStatusCode.OK);
                (await listed.JsonAsync<FunctionResponse[]>())!.ShouldContain(item =>
                    item.Id == function.Id && item.IsStaffed
                );
                await page.GotoAsync("/");
                await page.ScreenshotAsync(
                    new()
                    {
                        Path = $"playwright-artifacts/crew-planning-{RunStamp}.png",
                        FullPage = true,
                    }
                );
            }
        );
    }

    private static async Task<IdResponse> CreateAsync(IPage page, string route, object data)
    {
        var response = await page.APIRequest.PostAsync(route, new() { DataObject = data });
        response.Status.ShouldBe((int)HttpStatusCode.Created);
        return (await response.JsonAsync<IdResponse>())!;
    }

    private sealed record IdResponse([property: JsonPropertyName("id")] Guid Id);

    private sealed record FunctionResponse(
        [property: JsonPropertyName("id")] Guid Id,
        [property: JsonPropertyName("isStaffed")] bool IsStaffed,
        [property: JsonPropertyName("shifts")] ShiftResponse[] Shifts,
        [property: JsonPropertyName("crew")] CrewAssignmentResponse[] Crew
    );

    private sealed record ShiftResponse([property: JsonPropertyName("date")] string Date);

    private sealed record CrewAssignmentResponse(
        [property: JsonPropertyName("crewMemberId")] Guid CrewMemberId,
        [property: JsonPropertyName("rateId")] Guid? RateId,
        [property: JsonPropertyName("isProjectLeader")] bool IsProjectLeader
    );
}
#pragma warning restore MA0051, CA1861
