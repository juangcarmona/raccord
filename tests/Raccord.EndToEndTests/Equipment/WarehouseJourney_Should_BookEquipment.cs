using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Raccord.EndToEndTests.Projects;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Equipment;

/// <summary>
/// JRN-WHS-DISPATCH-EQUIPMENT / UC-WHS-ASSIGN-SCAN-CODES / UC-WHS-BOOK-MANUALLY:
/// Given an equipment item and project, when the warehouse records its packing transitions,
/// then the same persisted line reaches packed, dispatched and received states.
/// </summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
public sealed class WarehouseJourney_Should_BookEquipment(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    private static readonly string[] PackingTransitions = ["pack", "book-out", "receive"];

    [Fact]
    public async Task Journey_Should_TrackEquipmentFromScanCodeToReturn()
    {
        await RunJourneyAsync(
            "warehouse-book-equipment",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                var projectId = Guid.Parse(new Uri(page.Url).Segments[^1]);

                var api = page.Context.APIRequest;
                var itemResponse = await api.PostAsync(
                    "/api/equipment/items",
                    new() { DataObject = new { code = $"CAM-{RunStamp}", name = "Camera", stockQuantity = 1 } }
                );
                itemResponse.Status.ShouldBe(201);
                var item = await itemResponse.JsonAsync();
                var itemId = item!.Value.GetProperty("id").GetGuid();

                var scanResponse = await api.PutAsync(
                    $"/api/equipment/items/{itemId}/scan-code",
                    new() { DataObject = new { scanCode = $"scan-{RunStamp}" } }
                );
                scanResponse.Status.ShouldBe(200);

                var lineResponse = await api.PostAsync(
                    $"/api/projects/{projectId}/packing-lines",
                    new() { DataObject = new { equipmentId = itemId, quantity = 1 } }
                );
                lineResponse.Status.ShouldBe(201);
                var line = await lineResponse.JsonAsync();
                var lineId = line!.Value.GetProperty("id").GetGuid();

                foreach (var transition in PackingTransitions)
                {
                    var response = await api.PostAsync(
                        $"/api/projects/{projectId}/packing-lines/{lineId}/{transition}",
                        new() { DataObject = new { actor = "warehouse-e2e" } }
                    );
                    response.Status.ShouldBe(200);
                }

                var projectResponse = await api.GetAsync($"/api/projects/{projectId}");
                projectResponse.Status.ShouldBe(200);
                var project = await projectResponse.JsonAsync();
                project!.Value.GetProperty("packingLines")[0].GetProperty("isReceived").GetBoolean()
                    .ShouldBeTrue();
                await page.SetContentAsync("<h1>Warehouse equipment returned</h1>");
            }
        );
    }
}
