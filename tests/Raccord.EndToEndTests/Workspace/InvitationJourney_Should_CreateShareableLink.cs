using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;

namespace Raccord.EndToEndTests.Workspace;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Workspace)]
public sealed class InvitationJourney_Should_CreateShareableLink(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public Task Journey_Should_CreateAnInvitationLink() =>
        RunJourneyAsync(
            "user-invitation",
            async page =>
            {
                await page.GotoAsync("/settings/invitations");
                await page.GetByRole(
                        AriaRole.Heading,
                        new() { Name = "Invite a person", Level = 1 }
                    )
                    .WaitForAsync();
                await page.GetByLabel("Email").FillAsync($"invite-{RunStamp}@example.com");
                await page.GetByLabel("Role").SelectOptionAsync("Crew");
                await page.GetByRole(AriaRole.Button, new() { Name = "Create invitation" })
                    .ClickAsync();
                await page.GetByRole(AriaRole.Status).GetByText("/join/").WaitForAsync();
                await page.ScreenshotAsync(
                    new PageScreenshotOptions
                    {
                        Path = "docs/product/design/evidence/issue-41/invitation-link.png",
                        FullPage = true,
                    }
                );
            }
        );
}
