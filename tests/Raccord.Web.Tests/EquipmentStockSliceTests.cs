using System.Net;
using System.Net.Http.Json;
using Raccord.Contracts.Equipment;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class EquipmentStockSliceTests(PostgresContainer postgres) : IAsyncLifetime
{
    private RaccordApi api = null!;
    private HttpClient client = null!;

    public async ValueTask InitializeAsync()
    {
        api = await RaccordApi.StartAsync(postgres);
        client = api.CreateClient();
    }

    public async ValueTask DisposeAsync()
    {
        client.Dispose();
        await api.DisposeAsync();
    }

    [Fact]
    public async Task Stock_Should_ScheduleMovementsAndReconcilePhysicalCounts()
    {
        var created = await client.PostAsJsonAsync(
            "/api/equipment/items",
            new CreateEquipmentItemRequest("CAM-COUNT", "Camera", 10)
        );
        created.StatusCode.ShouldBe(HttpStatusCode.Created);
        var item = (await created.Content.ReadFromJsonAsync<EquipmentItemResponse>())!;

        var movement = await client.PostAsJsonAsync(
            $"/api/equipment/items/{item.Id}/movements",
            new ScheduleStockMovementRequest(
                "inbound",
                4,
                new DateOnly(2026, 11, 1),
                "supplier delivery"
            )
        );
        movement.StatusCode.ShouldBe(HttpStatusCode.Created);
        var movementResponse = (await movement.Content.ReadFromJsonAsync<StockMovementResponse>())!;
        movementResponse.Direction.ShouldBe("inbound");
        movementResponse.Status.ShouldBe("planned");

        var count = await client.PostAsJsonAsync(
            $"/api/equipment/items/{item.Id}/counts",
            new ReconcileStockCountRequest(12, "Annual physical count")
        );
        count.StatusCode.ShouldBe(HttpStatusCode.Created);
        var countResponse = (await count.Content.ReadFromJsonAsync<StockCountResponse>())!;
        countResponse.PreviousQuantity.ShouldBe(10);
        countResponse.Difference.ShouldBe(2);

        var current = await client.GetFromJsonAsync<EquipmentItemResponse>(
            $"/api/equipment/items/{item.Id}"
        );
        current!.StockQuantity.ShouldBe(12);
        current.AvailableQuantity.ShouldBe(12);

        var movements = await client.GetFromJsonAsync<List<StockMovementResponse>>(
            $"/api/equipment/items/{item.Id}/movements"
        );
        movements!.ShouldContain(entry => entry.Id == movementResponse.Id);
        var counts = await client.GetFromJsonAsync<List<StockCountResponse>>(
            $"/api/equipment/items/{item.Id}/counts"
        );
        counts!.ShouldContain(entry => entry.Id == countResponse.Id);
    }
}
