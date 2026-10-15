using System.Net.Http.Headers;
using System.Net.Http.Json;
using Raccord.Modules.Equipment.Contracts;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class EquipmentDetailsTests(PostgresContainer postgres) : IAsyncLifetime
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
    public async Task Equipment_Should_PersistDetailsRelationshipsAndImageMetadata()
    {
        var item = await (
            await client.PostAsJsonAsync(
                "/api/equipment/items",
                new CreateEquipmentItemRequest("CAM-01", "Camera", EquipmentItemType.Rental, 1)
            )
        )
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<EquipmentItemSnapshot>();

        var details = await client.PutAsJsonAsync(
            $"/api/equipment/items/{item!.Id}/details",
            new UpdateEquipmentItemDetailsRequest("Cinema camera", [item.Id], [item.Id])
        );
        var updated = await details
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<EquipmentItemSnapshot>();
        updated!.Details.ShouldBe("Cinema camera");
        updated.AccessoryIds!.ShouldContain(item.Id);
        updated.AlternativeIds!.ShouldContain(item.Id);

        using var content = new MultipartFormDataContent();
        using var image = new ByteArrayContent([0x89, 0x50, 0x4e, 0x47]);
        image.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        content.Add(image, "file", "camera.png");
        var imageResponse = await client.PostAsync(
            $"/api/equipment/items/{item.Id}/image",
            content
        );
        var withImage = await imageResponse
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<EquipmentItemSnapshot>();

        withImage!.ImageKey.ShouldBe($"equipment/{item.Id:N}/camera.png");
    }

    [Fact]
    public async Task Equipment_Should_Generate_A_Printable_Label()
    {
        var item = await (
            await client.PostAsJsonAsync(
                "/api/equipment/items",
                new CreateEquipmentItemRequest(
                    "LABEL-01",
                    "Label camera",
                    EquipmentItemType.Rental,
                    1
                )
            )
        )
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<EquipmentItemSnapshot>();

        var response = await client.GetAsync($"/api/equipment/items/{item!.Id}/label");

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.ShouldBe("application/pdf");
        (await response.Content.ReadAsByteArrayAsync()).Take(8).ShouldBe("%PDF-1.7"u8.ToArray());
    }
}
