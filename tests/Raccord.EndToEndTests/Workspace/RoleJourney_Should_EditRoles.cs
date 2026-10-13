using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;

namespace Raccord.EndToEndTests.Workspace;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Workspace)]
public sealed class RoleJourney_Should_EditRoles(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public Task Journey_Should_CreateARestrictedCustomRole() =>
        RunJourneyAsync(
            "role-editor",
            async page =>
            {
                await page.GotoAsync("/settings/roles");
                await page.GetByRole(
                        AriaRole.Heading,
                        new() { Name = "Roles and permissions", Level = 1 }
                    )
                    .WaitForAsync();
                await page.GetByLabel("Role name").FillAsync($"Read-only {RunStamp}");
                await page.GetByLabel("projects:read").CheckAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = "Create role" }).ClickAsync();
                await page.GetByText($"Read-only {RunStamp}").WaitForAsync();
                await page.ScreenshotAsync(
                    new PageScreenshotOptions
                    {
                        Path = "docs/product/design/evidence/issue-40/role-editor.png",
                        FullPage = true,
                    }
                );
            }
        );
}
