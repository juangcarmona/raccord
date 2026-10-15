using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Integrations;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
public sealed class CalendarFeedJourney_Should_PublishPrivateIcs(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_IssuePrivateReadOnlyProjectFeed()
    {
        await RunJourneyAsync(
            "calendar-feed",
            async page =>
            {
                var issue = await page.APIRequest.PostAsync(
                    "/api/integrations/calendar-feeds",
                    new() { DataObject = new { stream = "all-projects" } }
                );
                issue.Status.ShouldBe((int)HttpStatusCode.Created);
                var feed = (await issue.JsonAsync<FeedResponse>())!;

                var response = await page.APIRequest.GetAsync(feed.Url);
                response.Status.ShouldBe((int)HttpStatusCode.OK);
                response.Headers["content-type"].ShouldContain("text/calendar");
                var calendar = await response.TextAsync();
                calendar.ShouldContain("BEGIN:VCALENDAR");
                calendar.ShouldContain("Rodaje campaña primavera");
                calendar.ShouldContain("END:VCALENDAR");

                await page.GotoAsync(feed.Url);
                await page.ScreenshotAsync(
                    new()
                    {
                        Path = $"playwright-artifacts/calendar-feed-{RunStamp}.png",
                        FullPage = true,
                    }
                );

                var revoke = await page.APIRequest.DeleteAsync(
                    $"/api/integrations/calendar-feeds/{feed.Id}"
                );
                revoke.Status.ShouldBe((int)HttpStatusCode.NoContent);
                var revoked = await page.APIRequest.GetAsync(feed.Url);
                revoked.Status.ShouldBe((int)HttpStatusCode.NotFound);
            }
        );
    }

    private sealed record FeedResponse(
        [property: JsonPropertyName("id")] Guid Id,
        [property: JsonPropertyName("url")] string Url,
        [property: JsonPropertyName("stream")] string Stream
    );
}
