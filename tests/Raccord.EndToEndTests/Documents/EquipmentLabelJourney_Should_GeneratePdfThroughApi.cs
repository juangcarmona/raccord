using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Documents;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Documents)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
public sealed class EquipmentLabelJourney_Should_GeneratePdfThroughApi(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_CreateEquipmentAndDownloadItsLabel()
    {
        await RunJourneyAsync(
            "equipment-label-generate",
            async page =>
            {
                var code = $"LABEL-{RunStamp}";
                var created = await page.APIRequest.PostAsync(
                    "/api/equipment/items",
                    new()
                    {
                        DataObject = new
                        {
                            code,
                            name = "E2E label camera",
                            type = 0,
                            stockQuantity = 1,
                            serialNumbers = Array.Empty<string>(),
                        },
                    }
                );
                created.Status.ShouldBe((int)HttpStatusCode.Created);
                var item = (await created.JsonAsync<EquipmentResponse>())!;

                var label = await page.APIRequest.GetAsync($"/api/equipment/items/{item.Id}/label");

                label.Status.ShouldBe((int)HttpStatusCode.OK);
                label.Headers["content-type"].ShouldContain("application/pdf");
                (await label.BodyAsync())[..4].ShouldBe([0x25, 0x50, 0x44, 0x46]);
            }
        );
    }

    private sealed record EquipmentResponse([property: JsonPropertyName("id")] Guid Id);
}
