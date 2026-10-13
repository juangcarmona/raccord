using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Authentication;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Authentication)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
public sealed class AuthenticationJourney_Should_ProtectApi(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_RequireSignInBeforeServingProjects()
    {
        await RunJourneyAsync(
            "authentication-sign-in",
            async page =>
            {
                var anonymousResponse = await page.GotoAsync("/api/projects");
                Assert.Equal(401, anonymousResponse!.Status);

                await page.GotoAsync("/sign-in");
                await page.GetByLabel("Email").FillAsync("admin@example.com");
                await page.GetByLabel("Password").FillAsync("DemoPassword123!");
                await page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
                await page.WaitForURLAsync(url =>
                    url.EndsWith("/projects", StringComparison.Ordinal)
                );

                Assert.Contains(
                    "admin@example.com",
                    await page.Locator("header").InnerTextAsync(),
                    StringComparison.Ordinal
                );

                var authenticatedResponse = await page.GotoAsync("/api/projects");
                Assert.Equal(200, authenticatedResponse!.Status);

                await page.GetByRole(AriaRole.Button, new() { Name = "Sign out" }).ClickAsync();
                await page.WaitForURLAsync(url =>
                    url.EndsWith("/sign-in", StringComparison.Ordinal)
                );
                await page.GotoAsync("/projects");
                await page.WaitForURLAsync(url =>
                    url.EndsWith("/sign-in", StringComparison.Ordinal)
                );
            },
            signIn: false
        );
    }
}
