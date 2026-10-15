using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Crew;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
public sealed class WorkdayJourney_Should_RecordAuditableEvents(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
#pragma warning disable MA0051
    [Fact]
    public async Task Journey_Should_RecordClockInAndOutWithAuditData()
    {
        await RunJourneyAsync(
            "workday-events",
            async page =>
            {
                var response = await page.APIRequest.PostAsync(
                    "/api/crew/members",
                    new()
                    {
                        DataObject = new
                        {
                            firstName = $"Clock {RunStamp}",
                            role = "Operator",
                            email = $"clock-{RunStamp}@example.com",
                        },
                    }
                );
                response.Status.ShouldBe((int)HttpStatusCode.Created);
                var crew = (await response.JsonAsync<IdResponse>())!;
                var clockIn = await page.APIRequest.PostAsync(
                    "/api/crew/workday-events",
                    new()
                    {
                        DataObject = new
                        {
                            crewMemberId = crew.Id,
                            kind = 0,
                            occurredAt = "2026-09-15T08:00:00Z",
                            actor = "planner@example.com",
                        },
                    }
                );
                clockIn.Status.ShouldBe((int)HttpStatusCode.Created);
                var clockOut = await page.APIRequest.PostAsync(
                    "/api/crew/workday-events",
                    new()
                    {
                        DataObject = new
                        {
                            crewMemberId = crew.Id,
                            kind = 1,
                            occurredAt = "2026-09-15T17:00:00Z",
                            actor = "planner@example.com",
                            remark = "End of shift",
                        },
                    }
                );
                clockOut.Status.ShouldBe((int)HttpStatusCode.Created);
                var listed = await page.APIRequest.GetAsync($"/api/crew/workday-events/{crew.Id}");
                listed.Status.ShouldBe((int)HttpStatusCode.OK);
                var events = (await listed.JsonAsync<WorkdayEventResponse[]>())!;
                events.Length.ShouldBe(2);
                events.ShouldContain(item =>
                    item.Kind == 0
                    && item.Actor == "planner@example.com"
                    && item.RecordedAt != default
                );
                events.ShouldContain(item => item.Kind == 1 && item.Remark == "End of shift");
                await page.GotoAsync("/");
                await page.ScreenshotAsync(
                    new()
                    {
                        Path = $"playwright-artifacts/workday-events-{RunStamp}.png",
                        FullPage = true,
                    }
                );
            }
        );
    }
#pragma warning restore MA0051

    private sealed record IdResponse([property: JsonPropertyName("id")] Guid Id);

    private sealed record WorkdayEventResponse(
        [property: JsonPropertyName("kind")] int Kind,
        [property: JsonPropertyName("actor")] string Actor,
        [property: JsonPropertyName("recordedAt")] DateTimeOffset RecordedAt,
        [property: JsonPropertyName("remark")] string? Remark
    );
}
