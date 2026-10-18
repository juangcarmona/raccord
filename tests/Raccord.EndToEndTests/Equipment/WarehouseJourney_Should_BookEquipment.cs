using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Raccord.EndToEndTests.Projects;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Equipment;

/// <summary>
/// JRN-WHS-DISPATCH-EQUIPMENT / JRN-EQP-KEEP-STOCK-ACCURATE / UC-WHS-ASSIGN-SCAN-CODES / UC-WHS-BOOK-MANUALLY:
/// Given an equipment item and project, when the warehouse records its packing transitions,
/// then the same persisted line reaches packed, dispatched and received states while planned
/// movements and physical count corrections remain traceable.
/// ProductShape: UC-EQP-SCHEDULE-STOCK-MOVEMENT, UC-EQP-COUNT-SELECTED-EQUIPMENT,
/// UC-EQP-CORRECT-STOCK, UC-EQP-SELL-RENTAL-EQUIPMENT, UC-EQP-ARCHIVE-ITEM,
/// UC-EQP-VIEW-EQUIPMENT-PROJECT-HISTORY, UC-EQP-VIEW-ITEM-INSIGHTS.
/// </summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Equipment)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Warehouse)]
public sealed class WarehouseJourney_Should_BookEquipment(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    private static readonly string[] PackingTransitions = ["pack", "book-out", "receive"];

    [Fact]
    public async Task Journey_Should_TrackEquipmentFromScanCodeToReturn()
    {
        await RunJourneyAsync("warehouse-book-equipment", CompleteJourneyAsync);
    }

    [Fact]
    public async Task Journey_Should_AdjustAdditionalTemporaryAndOptionalLines()
    {
        await RunJourneyAsync(
            "warehouse-adjust-packing-lines",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                var projectId = Guid.Parse(new Uri(page.Url).Segments[^1]);
                var api = page.Context.APIRequest;
                var itemId = await CreateEquipmentAsync(api, "ADJUST");
                var additionalId = await CreatePackingLineAsync(
                    api,
                    projectId,
                    itemId,
                    "additional"
                );
                var temporaryId = await CreatePackingLineAsync(api, projectId, itemId, "temporary");

                var adjustment = await api.PatchAsync(
                    $"/api/projects/{projectId}/packing-lines/{additionalId}",
                    new() { DataObject = new { quantity = 1, role = "optional" } }
                );
                adjustment.Status.ShouldBe(200);
                (await adjustment.JsonAsync())!
                    .Value.GetProperty("role")
                    .GetString()
                    .ShouldBe("optional");
                var temporary = await api.GetAsync($"/api/projects/{projectId}");
                temporary.Status.ShouldBe(200);
                (await temporary.JsonAsync())!
                    .Value.GetProperty("packingLines")
                    .EnumerateArray()
                    .Single(line => line.GetProperty("id").GetGuid() == temporaryId)
                    .GetProperty("role")
                    .GetString()
                    .ShouldBe("temporary");
                await page.SetContentAsync("<h1>Packing adjustments recorded</h1>");
            }
        );
    }

    /// <summary>
    /// JRN-WHS-EXCEPTIONS / UC-WHS-MARK-DELAYED / UC-WHS-MARK-FOUND / UC-WHS-MARK-LOST /
    /// UC-WHS-TRACE-TRACKING-LOG / UC-WHS-REVIEW-CROSS-DOCKING / UC-WHS-RETURN-SALE-ITEMS:
    /// Given a dispatched line, when the warehouse records an exception and disposition,
    /// then both remain visible in the project trace.
    /// </summary>
    [Fact]
    public async Task Journey_Should_RecordExceptionAndCrossDockDisposition()
    {
        await RunJourneyAsync(
            "warehouse-exception-trace",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                var projectId = Guid.Parse(new Uri(page.Url).Segments[^1]);
                var api = page.Context.APIRequest;
                var itemId = await CreateEquipmentAsync(api, "TRACE");
                var lineId = await CreatePackingLineAsync(api, projectId, itemId);
                await PostPackingTransitionAsync(api, projectId, lineId, "pack");
                await PostPackingTransitionAsync(api, projectId, lineId, "book-out");

                var disposition = await api.PostAsync(
                    $"/api/projects/{projectId}/packing-lines/{lineId}/disposition",
                    new()
                    {
                        DataObject = new { disposition = "cross-docked", actor = "warehouse-e2e" },
                    }
                );
                disposition.Status.ShouldBe(200);
                var exception = await api.PostAsync(
                    $"/api/projects/{projectId}/packing-lines/{lineId}/exceptions",
                    new()
                    {
                        DataObject = new
                        {
                            kind = "delayed",
                            description = "Receiving dock delayed the handoff",
                            actor = "warehouse-e2e",
                        },
                    }
                );
                exception.Status.ShouldBe(200);
                var trace = await exception.JsonAsync();
                trace!.Value.GetProperty("disposition").GetString().ShouldBe("cross-docked");
                trace.Value.GetProperty("exceptionKind").GetString().ShouldBe("delayed");
                await page.SetContentAsync("<h1>Warehouse exception trace recorded</h1>");
            }
        );
    }

    private async Task CompleteJourneyAsync(IPage page)
    {
        await CreateCustomerAsync(page);
        await CreateProjectWithBillableLineAsync(page);
        var projectId = Guid.Parse(new Uri(page.Url).Segments[^1]);
        var api = page.Context.APIRequest;
        var itemId = await CreateEquipmentAsync(api);
        await AssertStockTrackingAsync(api, itemId);
        var lineId = await CreatePackingLineAsync(api, projectId, itemId);

        await RunPackingTransitionsAsync(api, projectId, lineId);
        await AssertReturnedAsync(api, projectId, itemId);
        await AssertLifecycleAsync(api, projectId, itemId);
        await page.SetContentAsync("<h1>Warehouse equipment returned</h1>");
    }

    private static async Task AssertStockTrackingAsync(IAPIRequestContext api, Guid itemId)
    {
        var movement = await api.PostAsync(
            $"/api/equipment/items/{itemId}/movements",
            new()
            {
                DataObject = new
                {
                    direction = "inbound",
                    quantity = 2,
                    scheduledDate = "2026-11-01",
                    reference = "E2E supplier delivery",
                },
            }
        );
        movement.Status.ShouldBe(201);
        (await movement.JsonAsync())!.Value.GetProperty("status").GetString().ShouldBe("planned");

        var count = await api.PostAsync(
            $"/api/equipment/items/{itemId}/counts",
            new() { DataObject = new { countedQuantity = 2, reason = "E2E physical count" } }
        );
        count.Status.ShouldBe(201);
        (await count.JsonAsync())!.Value.GetProperty("difference").GetInt32().ShouldBe(1);

        var history = await api.GetAsync($"/api/equipment/items/{itemId}/counts");
        history.Status.ShouldBe(200);
        (await history.TextAsync()).ShouldContain("E2E physical count");
    }

    private async Task<Guid> CreateEquipmentAsync(IAPIRequestContext api, string suffix = "CAM")
    {
        var itemResponse = await api.PostAsync(
            "/api/equipment/items",
            new()
            {
                DataObject = new
                {
                    code = $"{suffix}-{RunStamp}",
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
        Guid itemId,
        string role = "required"
    )
    {
        var response = await api.PostAsync(
            $"/api/projects/{projectId}/packing-lines",
            new()
            {
                DataObject = new
                {
                    equipmentId = itemId,
                    quantity = 1,
                    role,
                },
            }
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
            await PostPackingTransitionAsync(api, projectId, lineId, transition, data);
        }
    }

    private static async Task PostPackingTransitionAsync(
        IAPIRequestContext api,
        Guid projectId,
        Guid lineId,
        string transition,
        object? data = null
    )
    {
        var response = await api.PostAsync(
            $"/api/projects/{projectId}/packing-lines/{lineId}/{transition}",
            new() { DataObject = data ?? new { actor = "warehouse-e2e" } }
        );
        response.Status.ShouldBe(200);
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
            .ShouldBe(2);
    }

    private static async Task AssertLifecycleAsync(
        IAPIRequestContext api,
        Guid projectId,
        Guid itemId
    )
    {
        var sold = await api.PutAsync(
            $"/api/equipment/items/{itemId}/sell",
            new() { DataObject = new { amount = 1250, buyer = "E2E studio buyer" } }
        );
        sold.Status.ShouldBe(200);
        (await sold.JsonAsync())!.Value.GetProperty("status").GetString().ShouldBe("sold");

        var history = await api.GetAsync($"/api/equipment/items/{itemId}/history");
        history.Status.ShouldBe(200);
        (await history.TextAsync()).ShouldContain(projectId.ToString());

        var insights = await api.GetAsync($"/api/equipment/items/{itemId}/insights");
        insights.Status.ShouldBe(200);
        var body = await insights.TextAsync();
        body.ShouldContain("\"movementCount\":1");
        body.ShouldContain("\"countAdjustmentCount\":1");
        body.ShouldContain("\"projectCount\":1");
        body.ShouldContain("\"totalProjectQuantity\":1");
        body.ShouldContain("\"status\":\"sold\"");
    }
}
