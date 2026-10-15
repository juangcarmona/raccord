using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Crew;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
public sealed class CrewRecordsJourney_Should_ManageCrewVehiclesAndRates(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    private static readonly string[] CrewSkills = ["Rigging"];
    private static readonly string[] VehicleTags = ["Van"];

#pragma warning disable MA0051
    [Fact]
    public async Task Journey_Should_SaveCrewVehicleAndDefaultCostRate()
    {
        await RunJourneyAsync(
            "crew-records",
            async page =>
            {
                var crewResponse = await page.APIRequest.PostAsync(
                    "/api/crew/members",
                    new()
                    {
                        DataObject = new
                        {
                            firstName = $"Alex {RunStamp}",
                            role = "Stage manager",
                            email = $"alex-{RunStamp}@example.com",
                            skills = CrewSkills,
                        },
                    }
                );
                crewResponse.Status.ShouldBe((int)HttpStatusCode.Created);
                var crew = (await crewResponse.JsonAsync<CrewResponse>())!;
                crew.Skills.ShouldContain("Rigging");

                var vehicleResponse = await page.APIRequest.PostAsync(
                    "/api/crew/vehicles",
                    new()
                    {
                        DataObject = new
                        {
                            registrationNumber = $"E2E-{RunStamp}",
                            seats = 3,
                            tags = VehicleTags,
                            isShownInPlanning = false,
                        },
                    }
                );
                vehicleResponse.Status.ShouldBe((int)HttpStatusCode.Created);
                var vehicle = (await vehicleResponse.JsonAsync<VehicleResponse>())!;
                vehicle.IsShownInPlanning.ShouldBeFalse();

                var rateResponse = await page.APIRequest.PostAsync(
                    "/api/crew/rates",
                    new()
                    {
                        DataObject = new
                        {
                            name = $"E2E cost {RunStamp}",
                            kind = 1,
                            bands = new[]
                            {
                                new
                                {
                                    fromHour = 0,
                                    fixedAmount = 100,
                                    hourlyAmount = 25,
                                },
                            },
                            isDefault = true,
                        },
                    }
                );
                rateResponse.Status.ShouldBe((int)HttpStatusCode.Created);
                var rate = (await rateResponse.JsonAsync<RateResponse>())!;
                rate.IsDefault.ShouldBeTrue();

                var listedCrew = await page.APIRequest.GetAsync("/api/crew/members");
                var listedVehicles = await page.APIRequest.GetAsync("/api/crew/vehicles");
                var listedRates = await page.APIRequest.GetAsync("/api/crew/rates");
                listedCrew.Status.ShouldBe((int)HttpStatusCode.OK);
                listedVehicles.Status.ShouldBe((int)HttpStatusCode.OK);
                listedRates.Status.ShouldBe((int)HttpStatusCode.OK);
                (await listedCrew.JsonAsync<CrewResponse[]>())!.ShouldContain(item =>
                    item.Id == crew.Id
                );
                (await listedVehicles.JsonAsync<VehicleResponse[]>())!.ShouldContain(item =>
                    item.Id == vehicle.Id
                );
                (await listedRates.JsonAsync<RateResponse[]>())!.ShouldContain(item =>
                    item.Id == rate.Id
                );

                await page.GotoAsync("/");
                await page.ScreenshotAsync(
                    new()
                    {
                        Path = $"playwright-artifacts/crew-records-{RunStamp}.png",
                        FullPage = true,
                    }
                );
            }
        );
    }

#pragma warning restore MA0051

    private sealed record CrewResponse(
        [property: JsonPropertyName("id")] Guid Id,
        [property: JsonPropertyName("skills")] string[] Skills
    );

    private sealed record VehicleResponse(
        [property: JsonPropertyName("id")] Guid Id,
        [property: JsonPropertyName("isShownInPlanning")] bool IsShownInPlanning
    );

    private sealed record RateResponse(
        [property: JsonPropertyName("id")] Guid Id,
        [property: JsonPropertyName("isDefault")] bool IsDefault
    );
}
