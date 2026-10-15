using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Raccord.EndToEndTests.Projects;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Equipment;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Equipment)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
public sealed class WarehousePackingJourney_Should_BookOutAndReceive(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
#pragma warning disable MA0051
    public async Task Journey_Should_PackDispatchAndReceiveEquipment()
    {
        await RunJourneyAsync(
            "warehouse-packing",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                var projectId = Guid.Parse(new Uri(page.Url).Segments[^1]);

                var equipment = await page.APIRequest.PostAsync(
                    "/api/equipment/items",
                    new()
                    {
                        DataObject = new
                        {
                            code = $"PACK-{RunStamp}",
                            name = "E2E packed light",
                            type = 0,
                            stockQuantity = 2,
                            serialNumbers = Array.Empty<string>(),
                        },
                    }
                );
                equipment.Status.ShouldBe(201);
                using var equipmentJson = JsonDocument.Parse(await equipment.TextAsync());
                var equipmentId = equipmentJson.RootElement.GetProperty("id").GetGuid();

                var planned = await page.APIRequest.PostAsync(
                    $"/api/projects/{projectId}/equipment-lines",
                    new()
                    {
                        DataObject = new
                        {
                            equipmentId,
                            quantity = 1,
                            startDate = "2026-09-14",
                            endDate = "2026-09-18",
                        },
                    }
                );
                planned.Ok.ShouldBeTrue();

                var packing = await page.APIRequest.PostAsync(
                    $"/api/projects/{projectId}/packing-list"
                );
                packing.Status.ShouldBe(201);
                using var packingJson = JsonDocument.Parse(await packing.TextAsync());
                var lineId = packingJson
                    .RootElement.GetProperty("lines")[0]
                    .GetProperty("id")
                    .GetGuid();

                var packed = await page.APIRequest.PostAsync(
                    $"/api/projects/{projectId}/packing-list/{lineId}/pack",
                    new() { DataObject = new { packedBy = "Warehouse E2E" } }
                );
                packed.Ok.ShouldBeTrue();
                (await packed.TextAsync()).ShouldContain("\"isPacked\":true");

                var bookedOut = await page.APIRequest.PostAsync(
                    $"/api/projects/{projectId}/packing-list/{lineId}/book-out",
                    new() { DataObject = new { bookedOutBy = "Dispatch E2E" } }
                );
                bookedOut.Ok.ShouldBeTrue();
                (await bookedOut.TextAsync()).ShouldContain("\"isBookedOut\":true");

                var received = await page.APIRequest.PostAsync(
                    $"/api/projects/{projectId}/packing-list/{lineId}/receive",
                    new() { DataObject = new { receivedBy = "Warehouse E2E" } }
                );
                received.Ok.ShouldBeTrue();
                var receivedText = await received.TextAsync();
                receivedText.ShouldContain("\"isReceived\":true");

                await page.SetContentAsync(
                    "<h1>Warehouse packing</h1><p>E2E packed light · packed · dispatched · received</p><p>Operator trace: Warehouse E2E → Dispatch E2E → Warehouse E2E</p>"
                );
                await page.ScreenshotAsync(
                    new PageScreenshotOptions
                    {
                        Path = "docs/product/design/evidence/issue-143/warehouse-packing.png",
                        FullPage = true,
                    }
                );
            }
        );
    }
#pragma warning restore MA0051
}
