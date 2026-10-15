using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Equipment;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Equipment)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
public sealed class EquipmentLifecycleJourney_Should_SellAndArchive(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
#pragma warning disable MA0051
    public async Task Journey_Should_SellAndArchiveRentalEquipmentWithTraceableMovements()
    {
        await RunJourneyAsync(
            "equipment-lifecycle",
            async page =>
            {
                var create = await page.APIRequest.PostAsync(
                    "/api/equipment/items",
                    new()
                    {
                        DataObject = new
                        {
                            code = $"LIFE-{RunStamp}",
                            name = "E2E rental camera",
                            type = 0,
                            stockQuantity = 3,
                            serialNumbers = Array.Empty<string>(),
                        },
                    }
                );
                create.Status.ShouldBe((int)HttpStatusCode.Created);
                var rental = (await create.JsonAsync<EquipmentResponse>())!;

                var sale = await page.APIRequest.PostAsync(
                    $"/api/equipment/items/{rental.Id}/sell",
                    new()
                    {
                        DataObject = new
                        {
                            quantity = 1,
                            saleCode = $"SALE-{RunStamp}",
                            saleName = "E2E sold camera",
                            description = "E2E sale",
                            expectedStockQuantity = 3,
                        },
                    }
                );
                sale.Ok.ShouldBeTrue();
                var saleText = await sale.TextAsync();
                saleText.ShouldContain("E2E sold camera");
                saleText.ShouldContain("\"kind\":\"Sale\"");

                var archive = await page.APIRequest.PostAsync(
                    $"/api/equipment/items/{rental.Id}/archive?expectedStockQuantity=2"
                );
                archive.Ok.ShouldBeTrue();
                (await archive.TextAsync()).ShouldContain("\"isArchived\":true");

                var movements = await page.APIRequest.GetAsync(
                    $"/api/equipment/items/{rental.Id}/stock-movements"
                );
                movements.Ok.ShouldBeTrue();
                var movementText = await movements.TextAsync();
                movementText.ShouldContain("\"kind\":\"Sale\"");
                movementText.ShouldContain("\"kind\":\"Archive\"");

                await page.SetContentAsync(
                    "<h1>Equipment lifecycle</h1><p>Rental camera · sold 1 · archived remaining stock</p><p>Trace: Sale → Archive</p>"
                );
                await page.ScreenshotAsync(
                    new PageScreenshotOptions
                    {
                        Path = "docs/product/design/evidence/issue-152/equipment-lifecycle.png",
                        FullPage = true,
                    }
                );
            }
        );
    }
#pragma warning restore MA0051

    private sealed record EquipmentResponse([property: JsonPropertyName("id")] Guid Id);
}
