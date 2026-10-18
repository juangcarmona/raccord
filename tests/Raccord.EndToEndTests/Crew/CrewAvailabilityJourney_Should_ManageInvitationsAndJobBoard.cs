using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Crew;

/// Given a crew member and a planning function, when availability, an invitation and a job-board signup are recorded, then the crew response remains available to the planner.
/// ProductShape: BC-CREW-TRANSPORT, JRN-CRW-FILL-VIA-JOB-BOARD, UC-CRW-DECLARE-OWN-AVAILABILITY, UC-CRW-INVITE-CREW-FOR-FUNCTION, UC-CRW-RESPOND-TO-INVITATION, UC-CRW-POST-JOB-ON-BOARD, UC-CRW-SIGN-UP-FOR-POSTED-JOB.
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Crew)]
public sealed class CrewAvailabilityJourney_Should_ManageInvitationsAndJobBoard(
    DistributedAppFixture app
) : BrowserJourneyBase(app)
{
#pragma warning disable MA0051, CA1861
    [Fact]
    public async Task Journey_Should_RecordAvailabilityInviteAndJobSignup()
    {
        await RunJourneyAsync(
            "crew-availability",
            async page =>
            {
                var crewResponse = await page.APIRequest.PostAsync(
                    "/api/crew/members",
                    new()
                    {
                        DataObject = new
                        {
                            firstName = $"Available {RunStamp}",
                            role = "Camera operator",
                            email = $"available-{RunStamp}@example.com",
                        },
                    }
                );
                crewResponse.Status.ShouldBe((int)HttpStatusCode.Created);
                var crew = (await crewResponse.JsonAsync<IdResponse>())!;
                var availability = await page.APIRequest.PostAsync(
                    "/api/crew/availability",
                    new()
                    {
                        DataObject = new
                        {
                            crewMemberId = crew.Id,
                            startDate = "2026-09-14",
                            endDate = "2026-09-18",
                            kind = 0,
                            remark = "Available for the whole shoot",
                        },
                    }
                );
                availability.Status.ShouldBe((int)HttpStatusCode.Created);
                var functionId = Guid.NewGuid();
                var invitation = await page.APIRequest.PostAsync(
                    $"/api/projects/functions/{functionId}/invitations",
                    new() { DataObject = new { crewMemberId = crew.Id, remark = "Please confirm" } }
                );
                invitation.Status.ShouldBe((int)HttpStatusCode.Created);
                var invitationId = (await invitation.JsonAsync<InvitationResponse>())!.Id;
                var response = await page.APIRequest.PutAsync(
                    $"/api/projects/invitations/{invitationId}/response",
                    new() { DataObject = new { status = 1 } }
                );
                response.Status.ShouldBe((int)HttpStatusCode.OK);
                (await response.JsonAsync<InvitationResponse>())!.Status.ShouldBe(1);
                var job = await page.APIRequest.PostAsync(
                    $"/api/projects/functions/{functionId}/job-board",
                    new() { DataObject = new { brief = "Camera operator needed" } }
                );
                job.Status.ShouldBe((int)HttpStatusCode.Created);
                var jobId = (await job.JsonAsync<IdResponse>())!.Id;
                var signup = await page.APIRequest.PostAsync(
                    $"/api/projects/job-board/{jobId}/signups/{crew.Id}"
                );
                signup.Status.ShouldBe((int)HttpStatusCode.Created);
                var listed = await page.APIRequest.GetAsync($"/api/crew/availability/{crew.Id}");
                listed.Status.ShouldBe((int)HttpStatusCode.OK);
                (await listed.JsonAsync<AvailabilityResponse[]>())!.ShouldContain(item =>
                    item.CrewMemberId == crew.Id
                );
                await page.GotoAsync("/");
                await page.ScreenshotAsync(
                    new()
                    {
                        Path = $"playwright-artifacts/crew-availability-{RunStamp}.png",
                        FullPage = true,
                    }
                );
            }
        );
    }
#pragma warning restore MA0051, CA1861

    private sealed record IdResponse([property: JsonPropertyName("id")] Guid Id);

    private sealed record InvitationResponse(
        [property: JsonPropertyName("id")] Guid Id,
        [property: JsonPropertyName("status")] int Status
    );

    private sealed record AvailabilityResponse(
        [property: JsonPropertyName("crewMemberId")] Guid CrewMemberId
    );
}
