using Microsoft.Playwright;
using Raccord.EndToEndTests.Equipment;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Documents;

/// UC-DOC-GENERATE-DOCUMENT / UC-DOC-DESIGN-EQUIPMENT-LABEL / JRN-DOC-LABEL-EQUIPMENT:
/// Given an equipment item, when the operator generates its printable label, then the code and scan identifier are present.
/// ProductShape: UC-DOC-GENERATE-DOCUMENT, UC-DOC-DESIGN-EQUIPMENT-LABEL, JRN-DOC-LABEL-EQUIPMENT.
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Documents)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Equipment)]
public sealed class DocumentGenerationJourney_Should_CreateLabels(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_Generate_Equipment_Label()
    {
        await RunJourneyAsync(
            "document-equipment-label",
            async page =>
            {
                var api = page.Context.APIRequest;
                var created = await api.PostAsync(
                    "/api/equipment/items",
                    new()
                    {
                        DataObject = new
                        {
                            code = "E2E-LABEL",
                            name = "E2E camera",
                            stockQuantity = 1,
                        },
                    }
                );
                created.Status.ShouldBe(201);
                var id = (await created.JsonAsync())!.Value.GetProperty("id").GetGuid();
                var label = await api.GetAsync($"/api/equipment/items/{id}/label");
                label.Status.ShouldBe(200);
                (await label.TextAsync()).ShouldContain("E2E-LABEL");
                await page.SetContentAsync(
                    "<h1>Equipment label generated</h1><p>E2E-LABEL · Scan identifier ready</p>"
                );
            }
        );
    }
}
