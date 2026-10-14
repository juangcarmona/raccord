using System.Net;
using System.Net.Http.Json;
using Raccord.Modules.Equipment.Contracts;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class EquipmentLifecycleTests(PostgresContainer postgres) : IAsyncLifetime
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
    public async Task Selling_Rental_Equipment_Creates_Sale_Copy_And_Reduces_Rental_Stock()
    {
        var rental = await CreateItemAsync("CAM-SELL", 5);
        var response = await client.PostAsJsonAsync(
            $"/api/equipment/items/{rental.Id}/sell",
            new SellEquipmentRequest(2, "CAM-SALE", "Used camera")
        );
        var sale = await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<EquipmentSaleResponse>();

        sale.ShouldNotBeNull();
        sale!.RentalItem.StockQuantity.ShouldBe(3);
        sale.SaleItem.Type.ShouldBe(EquipmentItemType.Sale);
        sale.SaleItem.StockQuantity.ShouldBe(0);
        sale.Movement.Quantity.ShouldBe(-2);
    }

    [Fact]
    public async Task Archiving_Requires_Current_Stock_And_Preserves_The_Item()
    {
        var rental = await CreateItemAsync("CAM-ARCHIVE", 1);
        var conflict = await client.PostAsync(
            $"/api/equipment/items/{rental.Id}/archive?expectedStockQuantity=2",
            null
        );
        conflict.StatusCode.ShouldBe(HttpStatusCode.Conflict);

        var response = await client.PostAsync(
            $"/api/equipment/items/{rental.Id}/archive?expectedStockQuantity=1",
            null
        );
        var archived = await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<EquipmentItemSnapshot>();
        archived.ShouldNotBeNull();
        archived!.IsArchived.ShouldBeTrue();
        archived.StockQuantity.ShouldBe(0);
    }

    private async Task<EquipmentItemSnapshot> CreateItemAsync(string code, int stock) =>
        await (
            await client.PostAsJsonAsync(
                "/api/equipment/items",
                new CreateEquipmentItemRequest(code, "Camera", EquipmentItemType.Rental, stock)
            )
        )
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<EquipmentItemSnapshot>() ?? throw new InvalidOperationException();
}
