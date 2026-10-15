using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Documents;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Documents)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
public sealed class CommunicationJourney_Should_RecordDeliveryOutcomeThroughApi(
    DistributedAppFixture app
) : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_RecordFailureAndFindItInTheCommunicationLog()
    {
        await RunJourneyAsync(
            "communication-delivery-outcome",
            async page =>
            {
                var recipient = $"e2e-{RunStamp}@example.com";
                var created = await page.APIRequest.PostAsync(
                    "/api/documents/communications",
                    new()
                    {
                        DataObject = new
                        {
                            recipient,
                            subject = "E2E invoice",
                            documentKey = "invoices/e2e.pdf",
                        },
                    }
                );
                created.Status.ShouldBe((int)HttpStatusCode.Created);
                var communication = (await created.JsonAsync<CommunicationResponse>())!;
                communication.Status.ShouldBe("Pending");

                var failed = await page.APIRequest.PutAsync(
                    $"/api/documents/communications/{communication.Id}/delivery",
                    new()
                    {
                        DataObject = new
                        {
                            status = "Failed",
                            failureReason = "E2E mailbox rejection",
                        },
                    }
                );
                failed.Status.ShouldBe((int)HttpStatusCode.OK);
                var outcome = (await failed.JsonAsync<CommunicationResponse>())!;
                outcome.Status.ShouldBe("Failed");

                var search = await page.APIRequest.GetAsync(
                    $"/api/documents/communications?search={recipient}"
                );
                search.Status.ShouldBe((int)HttpStatusCode.OK);
                var results = (await search.JsonAsync<CommunicationResponse[]>())!;
                results.ShouldContain(item => item.Id == communication.Id);
            }
        );
    }

    private sealed record CommunicationResponse(
        [property: JsonPropertyName("id")] Guid Id,
        [property: JsonPropertyName("status")] string Status
    );
}
