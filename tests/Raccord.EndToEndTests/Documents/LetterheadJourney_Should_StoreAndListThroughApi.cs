using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Documents;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Documents)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
public sealed class LetterheadJourney_Should_StoreAndListThroughApi(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_StorePdfMetadataAndListTheLetterhead()
    {
        await RunJourneyAsync(
            "letterhead-store-and-list",
            async page =>
            {
                var multipart = page.APIRequest.CreateFormData();
                multipart.Set("name", $"E2E letterhead {RunStamp}");
                multipart.Set("documentTypes", "Invoice,Quote");
                multipart.Set("pageWidthMillimetres", "210");
                multipart.Set("pageHeightMillimetres", "297");
                multipart.Set("marginTopMillimetres", "12");
                multipart.Set("marginRightMillimetres", "10");
                multipart.Set("marginBottomMillimetres", "12");
                multipart.Set("marginLeftMillimetres", "10");
                multipart.Set(
                    "file",
                    new FilePayload
                    {
                        Name = "letterhead.pdf",
                        MimeType = "application/pdf",
                        Buffer = Encoding.UTF8.GetBytes("%PDF-1.7"),
                    }
                );

                var createdResponse = await page.APIRequest.PostAsync(
                    "/api/documents/letterheads",
                    new() { Multipart = multipart }
                );
                createdResponse.Status.ShouldBe((int)HttpStatusCode.OK);
                var created = (await createdResponse.JsonAsync<LetterheadResponse>())!;
                created.IsDefault.ShouldBeTrue();
                created.StorageKey.ShouldStartWith("letterheads/");

                var listedResponse = await page.APIRequest.GetAsync("/api/documents/letterheads");
                listedResponse.Status.ShouldBe((int)HttpStatusCode.OK);
                var listed = (await listedResponse.JsonAsync<LetterheadResponse[]>())!;
                listed.ShouldContain(letterhead => letterhead.Id == created.Id);
            }
        );
    }

    private sealed record LetterheadResponse(
        [property: JsonPropertyName("id")] Guid Id,
        [property: JsonPropertyName("isDefault")] bool IsDefault,
        [property: JsonPropertyName("storageKey")] string StorageKey
    );
}
