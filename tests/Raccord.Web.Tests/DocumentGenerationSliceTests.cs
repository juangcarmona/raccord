using System.Net;
using System.Net.Http.Json;
using Raccord.Contracts.Documents;
using Raccord.Contracts.Equipment;
using Raccord.Web.Tests.Infrastructure;
using Shouldly;

namespace Raccord.Web.Tests;

public sealed class DocumentGenerationSliceTests(PostgresContainer postgres) : IAsyncLifetime
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
    public async Task Documents_Should_Generate_Project_Summary_And_Equipment_Label()
    {
        var equipmentResponse = await client.PostAsJsonAsync(
            "/api/equipment/items",
            new CreateEquipmentItemRequest("DOC-LABEL", "Document camera", 1)
        );
        equipmentResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
        var equipment = (
            await equipmentResponse.Content.ReadFromJsonAsync<EquipmentItemResponse>()
        )!;
        var label = await client.GetFromJsonAsync<EquipmentLabelResponse>(
            $"/api/equipment/items/{equipment.Id}/label"
        );
        label!.Html.ShouldContain("DOC-LABEL");
        label.FileName.ShouldEndWith(".html");
    }
}
