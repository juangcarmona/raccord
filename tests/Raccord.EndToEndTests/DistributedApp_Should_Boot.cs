using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests;

/// <summary>
/// The one journey whose failure makes every other journey meaningless: the whole AppHost (Postgres,
/// Azurite, migrations, the API, the Vite frontend) boots, the SPA is served and the API-backed
/// projects list renders with migrated demo data behind it. The hosted API response also confirms
/// the production browser policy is same-origin and does not permit inline styles or wildcard sources.
/// This security-hardening journey has no ProductShape citation because it does not define product behavior.
/// </summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.CrossCutting)]
public sealed class DistributedApp_Should_Boot(DistributedAppFixture app) : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_ServeTheSpaAndTheSeededProjectsList()
    {
        await RunJourneyAsync(
            "distributed-app-boot",
            async page =>
            {
                var healthResponse = await page.Context.APIRequest.GetAsync(
                    new Uri(App.WebBaseUrl, "/health/ready").ToString()
                );
                healthResponse.Status.ShouldBe(200);
                var contentSecurityPolicy = healthResponse.Headers["content-security-policy"];
                contentSecurityPolicy.ShouldContain("style-src 'self'");
                contentSecurityPolicy.ShouldContain("connect-src 'self'");
                contentSecurityPolicy.ShouldNotContain("unsafe-inline");
                contentSecurityPolicy.ShouldNotContain("ws:");
                contentSecurityPolicy.ShouldNotContain("wss:");

                // The root redirects to /projects; the seeded demo projects render only when the API,
                // the database and the Vite /api proxy all work end-to-end.
                await page.GotoAsync("/");
                await page.GetByRole(AriaRole.Heading, new() { Name = "Projects", Level = 1 })
                    .WaitForAsync();
                await page.GetByText("Rodaje campaña primavera").WaitForAsync();
            }
        );
    }
}
