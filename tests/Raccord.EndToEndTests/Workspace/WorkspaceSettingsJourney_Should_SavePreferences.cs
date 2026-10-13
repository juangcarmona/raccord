using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;

namespace Raccord.EndToEndTests.Workspace;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Workspace)]
public sealed class WorkspaceSettingsJourney_Should_SavePreferences(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public Task Journey_Should_SaveLanguageAndTimeZone() =>
        RunJourneyAsync(
            "workspace-settings",
            async page =>
            {
                await page.GotoAsync("/settings/workspace");
                await page.GetByRole(
                        AriaRole.Heading,
                        new() { Name = "Workspace settings", Level = 1 }
                    )
                    .WaitForAsync();
                var settingsForm = page.Locator("form");
                await settingsForm.GetByLabel("Language").SelectOptionAsync("es");
                await settingsForm.GetByLabel("Time zone").FillAsync("Europe/Madrid");
                await settingsForm.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
                await page.GetByRole(AriaRole.Status)
                    .GetByText("Workspace settings saved.")
                    .WaitForAsync();
                await page.ScreenshotAsync(
                    new PageScreenshotOptions
                    {
                        Path = "docs/product/design/evidence/issue-42/workspace-settings.png",
                        FullPage = true,
                    }
                );
            }
        );
}
