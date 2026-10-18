using System.Net;
using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Integrations;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.EvidenceMode, E2ETraits.EvidenceModes.Api)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Contacts)]
public sealed class CompanyRegistryJourney_Should_HandleUnavailableProvider(
    DistributedAppFixture app
) : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_ReturnNoMatchesForUnconfiguredRegistry()
    {
        await RunJourneyAsync(
            "company-registry",
            async page =>
            {
                var response = await page.APIRequest.GetAsync(
                    "/api/integrations/company-registry?country=ES&query=B00000000"
                );
                response.Status.ShouldBe((int)HttpStatusCode.OK);
                (await response.JsonAsync<JsonElement>()).GetArrayLength().ShouldBe(0);
            }
        );
    }
}
