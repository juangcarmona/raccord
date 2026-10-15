using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests;

/// <summary>
/// The one journey whose failure makes every other journey meaningless: the whole AppHost (Postgres,
/// Azurite, migrations, the API, the Vite frontend) boots, the SPA is served and the API-backed
/// projects list renders with the migrated installation behind it.
/// </summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Invoicing)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
public sealed class DistributedApp_Should_Boot(DistributedAppFixture app) : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_ServeTheSpaAndTheProjectsList()
    {
        await RunJourneyAsync(
            "distributed-app-boot",
            async page =>
            {
                await page.GotoAsync("/projects");
                await page.GetByRole(AriaRole.Heading, new() { Name = "Projects", Level = 1 })
                    .WaitForAsync();
            }
        );
    }
}
