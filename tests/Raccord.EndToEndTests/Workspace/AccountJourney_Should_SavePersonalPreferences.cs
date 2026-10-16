using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Workspace;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Workspace)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
public sealed class AccountJourney_Should_SavePersonalPreferences(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_SaveProfileAndRestorePreferredLanguageAfterSignIn()
    {
        await RunJourneyAsync(
            "account-profile-preferences",
            async page =>
            {
                await page.GetByRole(AriaRole.Link, new() { Name = "My account" }).ClickAsync();
                await page.GetByRole(AriaRole.Heading, new() { Name = "My account" })
                    .WaitForAsync();
                await page.GetByLabel("Display name").FillAsync("Production Admin");
                await page.GetByLabel("Preferred language").SelectOptionAsync("en");
                await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
                await page.GetByText("Preferences saved.").WaitForAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "Manage roles and permissions" })
                    .WaitForAsync();

                await page.ReloadAsync();
                await page.GetByLabel("Display name").WaitForAsync();
                Assert.Equal(
                    "Production Admin",
                    await page.GetByLabel("Display name").InputValueAsync()
                );
                Assert.Equal("en", await page.GetByLabel("Preferred language").InputValueAsync());

                await page.Locator(".shell__toolbar button").ClickAsync();
                await page.WaitForURLAsync(url =>
                    url.EndsWith("/sign-in", StringComparison.Ordinal)
                );
                await page.GetByLabel("Email").FillAsync("admin@example.com");
                await page.GetByLabel("Password").FillAsync("DemoPassword123!");
                await page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
                await page.WaitForURLAsync(url =>
                    url.EndsWith("/projects", StringComparison.Ordinal)
                );
                await page.GetByRole(AriaRole.Heading, new() { Name = "Projects" }).WaitForAsync();

                var current = await page.EvaluateAsync<string>(
                    "async () => JSON.stringify(await (await fetch('/api/auth/me')).json())"
                );
                using var currentDocument = JsonDocument.Parse(current);
                Assert.Equal(
                    "Production Admin",
                    currentDocument.RootElement.GetProperty("displayName").GetString()
                );
                Assert.Equal(
                    "en",
                    currentDocument.RootElement.GetProperty("preferredLanguage").GetString()
                );
            }
        );
    }
}
