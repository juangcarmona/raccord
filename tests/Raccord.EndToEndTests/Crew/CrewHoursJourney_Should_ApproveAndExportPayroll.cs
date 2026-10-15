using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Crew;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
public sealed class CrewHoursJourney_Should_ApproveAndExportPayroll(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
#pragma warning disable MA0051
    [Fact]
    public async Task Journey_Should_ApproveHoursAndExportPayroll()
    {
        await RunJourneyAsync(
            "crew-hours",
            async page =>
            {
                var response = await page.APIRequest.PostAsync(
                    "/api/crew/members",
                    new()
                    {
                        DataObject = new
                        {
                            firstName = $"Payroll {RunStamp}",
                            role = "Technician",
                            email = $"payroll-{RunStamp}@example.com",
                        },
                    }
                );
                response.Status.ShouldBe((int)HttpStatusCode.Created);
                var crew = (await response.JsonAsync<IdResponse>())!;
                var hours = await page.APIRequest.PostAsync(
                    "/api/crew/hours",
                    new()
                    {
                        DataObject = new
                        {
                            crewMemberId = crew.Id,
                            workDate = "2026-09-15",
                            hours = 7.5,
                            source = 0,
                            remark = "Shoot day",
                        },
                    }
                );
                hours.Status.ShouldBe((int)HttpStatusCode.Created);
                var registration = (await hours.JsonAsync<HoursResponse>())!;
                registration.Status.ShouldBe(0);
                var pendingPayroll = await page.APIRequest.GetAsync(
                    "/api/crew/payroll-export?from=2026-09-01&to=2026-09-30"
                );
                (await pendingPayroll.TextAsync()).ShouldNotContain("7.5");
                var approval = await page.APIRequest.PutAsync(
                    $"/api/crew/hours/{registration.Id}/approval",
                    new() { DataObject = new { status = 1 } }
                );
                approval.Status.ShouldBe((int)HttpStatusCode.OK);
                (await approval.JsonAsync<HoursResponse>())!.Status.ShouldBe(1);
                var payroll = await page.APIRequest.GetAsync(
                    "/api/crew/payroll-export?from=2026-09-01&to=2026-09-30"
                );
                payroll.Status.ShouldBe((int)HttpStatusCode.OK);
                var csv = await payroll.TextAsync();
                csv.ShouldContain("crew_member_id,work_date,hours");
                csv.ShouldContain("7.5");
                csv.ShouldContain(crew.Id.ToString());
                await page.GotoAsync("/");
                await page.ScreenshotAsync(
                    new()
                    {
                        Path = $"playwright-artifacts/crew-hours-{RunStamp}.png",
                        FullPage = true,
                    }
                );
            }
        );
    }
#pragma warning restore MA0051

    private sealed record IdResponse([property: JsonPropertyName("id")] Guid Id);

    private sealed record HoursResponse(
        [property: JsonPropertyName("id")] Guid Id,
        [property: JsonPropertyName("status")] int Status
    );
}
