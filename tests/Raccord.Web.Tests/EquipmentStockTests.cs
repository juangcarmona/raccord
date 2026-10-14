using System.Net;
using System.Net.Http.Json;
using Raccord.Modules.Equipment.Contracts;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class EquipmentStockTests(PostgresContainer postgres) : IAsyncLifetime
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
    public async Task Equipment_Should_Record_Scheduled_Movement_And_Correction()
    {
        var item = await CreateItemAsync("LGT-01", 4);
        var scheduled = await client.PostAsJsonAsync(
            $"/api/equipment/items/{item.Id}/stock-movements",
            new ScheduleStockMovementRequest(
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)),
                "Inbound delivery"
            )
        );
        scheduled.StatusCode.ShouldBe(HttpStatusCode.OK);

        var correction = await client.PostAsJsonAsync(
            $"/api/equipment/items/{item.Id}/stock-correction",
            new CorrectStockRequest(5, 4, "Physical count")
        );
        correction.StatusCode.ShouldBe(HttpStatusCode.OK);

        var movements = await client.GetFromJsonAsync<StockMovementSnapshot[]>(
            $"/api/equipment/items/{item.Id}/stock-movements"
        );
        movements.ShouldNotBeNull();
        movements!.Select(movement => movement.Kind).ShouldContain("Scheduled");
        movements.Select(movement => movement.Kind).ShouldContain("Correction");
    }

    [Fact]
    public async Task Equipment_Should_Reject_Stale_Stock_Correction()
    {
        var item = await CreateItemAsync("LGT-02", 4);
        var response = await client.PostAsJsonAsync(
            $"/api/equipment/items/{item.Id}/stock-correction",
            new CorrectStockRequest(5, 3, "Stale count")
        );
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Equipment_Should_Complete_Selected_Stock_Count()
    {
        var first = await CreateItemAsync("LGT-03", 4);
        var second = await CreateItemAsync("LGT-04", 2);
        var response = await client.PostAsJsonAsync(
            "/api/equipment/stock-counts",
            new CompleteStockCountRequest(
                "October count",
                [new StockCountLine(first.Id, 4, 3), new StockCountLine(second.Id, 2, 4)]
            )
        );
        var results = await response
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<StockCountResult[]>();
        results.ShouldNotBeNull();
        results!.Select(result => result.Difference).ShouldBe([-1, 2]);
    }

    private async Task<EquipmentItemSnapshot> CreateItemAsync(string code, int stock) =>
        await (
            await client.PostAsJsonAsync(
                "/api/equipment/items",
                new CreateEquipmentItemRequest(code, "Light", EquipmentItemType.Rental, stock)
            )
        )
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<EquipmentItemSnapshot>()
        ?? throw new InvalidOperationException();
}
