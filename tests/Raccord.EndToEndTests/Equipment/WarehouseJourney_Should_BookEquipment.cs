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
        await RunJourneyAsync("warehouse-book-equipment", CompleteJourneyAsync);
    }

    private async Task CompleteJourneyAsync(IPage page)
    {
        await CreateCustomerAsync(page);
        await CreateProjectWithBillableLineAsync(page);
        var projectId = Guid.Parse(new Uri(page.Url).Segments[^1]);
        var api = page.Context.APIRequest;
        var itemId = await CreateEquipmentAsync(api);
        var lineId = await CreatePackingLineAsync(api, projectId, itemId);

        await RunPackingTransitionsAsync(api, projectId, lineId);
        await AssertReturnedAsync(api, projectId, itemId);
        await page.SetContentAsync("<h1>Warehouse equipment returned</h1>");
    }

    private async Task<Guid> CreateEquipmentAsync(IAPIRequestContext api)
    {
        var itemResponse = await api.PostAsync(
            "/api/equipment/items",
            new()
            {
                DataObject = new
                {
                    code = $"CAM-{RunStamp}",
                    name = "Camera",
                    stockQuantity = 1,
                },
            }
        );
        itemResponse.Status.ShouldBe(201);
        var item = await itemResponse.JsonAsync();
        var itemId = item!.Value.GetProperty("id").GetGuid();
        var scanResponse = await api.PutAsync(
            $"/api/equipment/items/{itemId}/scan-code",
            new() { DataObject = new { scanCode = $"scan-{RunStamp}" } }
        );
        scanResponse.Status.ShouldBe(200);
        return itemId;
    }

    private static async Task<Guid> CreatePackingLineAsync(
        IAPIRequestContext api,
        Guid projectId,
        Guid itemId
    )
    {
        var response = await api.PostAsync(
            $"/api/projects/{projectId}/packing-lines",
            new() { DataObject = new { equipmentId = itemId, quantity = 1 } }
        );
        response.Status.ShouldBe(201);
        var line = await response.JsonAsync();
        return line!.Value.GetProperty("id").GetGuid();
    }

    private async Task RunPackingTransitionsAsync(
        IAPIRequestContext api,
        Guid projectId,
        Guid lineId
    )
    {
        foreach (var transition in PackingTransitions)
        {
            object data = string.Equals(transition, "book-out", StringComparison.Ordinal)
                ? (object)new { actor = "warehouse-e2e", scanCode = $"scan-{RunStamp}" }
                : new { actor = "warehouse-e2e", scanCode = (string?)null };
            var response = await api.PostAsync(
                $"/api/projects/{projectId}/packing-lines/{lineId}/{transition}",
                new() { DataObject = data }
            );
            response.Status.ShouldBe(200);
        }
    }

    private static async Task AssertReturnedAsync(
        IAPIRequestContext api,
        Guid projectId,
        Guid itemId
    )
    {
        var projectResponse = await api.GetAsync($"/api/projects/{projectId}");
        projectResponse.Status.ShouldBe(200);
        var project = await projectResponse.JsonAsync();
        project!
            .Value.GetProperty("packingLines")[0]
            .GetProperty("isReceived")
            .GetBoolean()
            .ShouldBeTrue();
        var equipmentResponse = await api.GetAsync($"/api/equipment/items/{itemId}");
        equipmentResponse.Status.ShouldBe(200);
        (await equipmentResponse.JsonAsync())!
            .Value.GetProperty("availableQuantity")
            .GetInt32()
            .ShouldBe(1);
    }
}
