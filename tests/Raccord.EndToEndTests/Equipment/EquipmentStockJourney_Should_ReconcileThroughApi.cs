using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Equipment;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Equipment)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
public sealed class EquipmentStockJourney_Should_ReconcileThroughApi(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_RecordMovementAndRejectStaleCorrection()
    {
        await RunJourneyAsync(
            "equipment-stock-reconcile",
            async page =>
            {
                var code = $"E2E-{RunStamp}";
                var create = await page.APIRequest.PostAsync(
                    "/api/equipment/items",
                    new()
                    {
                        DataObject = new
                        {
                            code,
                            name = "E2E light",
                            type = 0,
                            stockQuantity = 4,
                            serialNumbers = Array.Empty<string>(),
                        },
                    }
                );
                create.Status.ShouldBe((int)HttpStatusCode.Created);
                var created = (await create.JsonAsync<EquipmentResponse>())!;

                var scheduled = await page.APIRequest.PostAsync(
                    $"/api/equipment/items/{created.Id}/stock-movements",
                    new()
                    {
                        DataObject = new
                        {
                            quantity = 2,
                            effectiveDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                            description = "E2E delivery",
                        },
                    }
                );
                scheduled.Status.ShouldBe((int)HttpStatusCode.OK);

                var stale = await page.APIRequest.PostAsync(
                    $"/api/equipment/items/{created.Id}/stock-correction",
                    new()
                    {
                        DataObject = new
                        {
                            stockQuantity = 3,
                            expectedStockQuantity = 2,
                            description = "E2E stale count",
                        },
                    }
                );
                stale.Status.ShouldBe((int)HttpStatusCode.Conflict);

                var movementsResponse = await page.APIRequest.GetAsync(
                    $"/api/equipment/items/{created.Id}/stock-movements"
                );
                var movements = (await movementsResponse.JsonAsync<MovementResponse[]>())!;
                movements.ShouldContain(movement => movement.Kind == "Scheduled");
            }
        );
    }

    private sealed record EquipmentResponse([property: JsonPropertyName("id")] Guid Id);

    private sealed record MovementResponse([property: JsonPropertyName("kind")] string Kind);
}
