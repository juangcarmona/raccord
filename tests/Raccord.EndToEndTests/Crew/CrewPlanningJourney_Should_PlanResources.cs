using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Crew;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
public sealed class CrewPlanningJourney_Should_PlanResources(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
#pragma warning disable MA0051
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

                var crewResponse = await page.APIRequest.PostAsync(
                    "/api/crew/members",
                    new()
                    {
                        DataObject = new
                        {
                            firstName = $"Planner {RunStamp}",
                            role = "Producer",
                            email = $"planner-{RunStamp}@example.com",
                        },
                    }
                );
                crewResponse.Status.ShouldBe((int)HttpStatusCode.Created);
                var crew = (await crewResponse.JsonAsync<IdResponse>())!;
                var vehicleResponse = await page.APIRequest.PostAsync(
                    "/api/crew/vehicles",
                    new()
                    {
                        DataObject = new { registrationNumber = $"PLAN-{RunStamp}", seats = 4 },
                    }
                );
                vehicleResponse.Status.ShouldBe((int)HttpStatusCode.Created);
                var vehicle = (await vehicleResponse.JsonAsync<IdResponse>())!;
                var rateResponse = await page.APIRequest.PostAsync(
                    "/api/crew/rates",
                    new()
                    {
                        DataObject = new
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
                        },
                    }
                );
                rateResponse.Status.ShouldBe((int)HttpStatusCode.Created);
                var rate = (await rateResponse.JsonAsync<IdResponse>())!;

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
                var shiftResponse = await page.APIRequest.PostAsync(
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
                shiftResponse.Status.ShouldBe((int)HttpStatusCode.OK);
                var assignResponse = await page.APIRequest.PostAsync(
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
                assignResponse.Status.ShouldBe((int)HttpStatusCode.OK);
                var planned = (await assignResponse.JsonAsync<FunctionResponse>())!;
                planned.IsStaffed.ShouldBeTrue();
                planned.Shifts.ShouldContain(shift => shift.Date == "2026-09-15");
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
                await page.GotoAsync($"/projects/{projectId}");
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
#pragma warning restore MA0051

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
