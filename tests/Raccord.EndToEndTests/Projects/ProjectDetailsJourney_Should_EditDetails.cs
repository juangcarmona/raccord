using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Projects;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
public sealed class ProjectDetailsJourney_Should_EditDetails(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_EditAndReloadProjectDetails()
    {
        await RunJourneyAsync(
            "project-edit-details",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);

                await page.GetByLabel("Project name").FillAsync("Summer campaign shoot");
                await page.GetByLabel("Start").FillAsync("2026-10-01");
                await page.GetByLabel("End").FillAsync("2026-10-03");
                await page.GetByRole(AriaRole.Button, new() { Name = "Save details" }).ClickAsync();
                await page.GetByRole(AriaRole.Heading, new() { Name = "Summer campaign shoot" })
                    .WaitForAsync();
                await page.ReloadAsync();

                (await page.GetByLabel("Project name").InputValueAsync()).ShouldBe(
                    "Summer campaign shoot"
                );
                (await page.GetByLabel("Start").InputValueAsync()).ShouldBe("2026-10-01");
                (await page.GetByLabel("End").InputValueAsync()).ShouldBe("2026-10-03");
            }
        );
    }
}
