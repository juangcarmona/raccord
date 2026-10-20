using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Projects;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
public sealed class ProjectLifecycleJourney_Should_ChangeStatus(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_ChangeAndReloadProjectStatus()
    {
        await RunJourneyAsync(
            "project-manual-status",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);

                var status = page.GetByRole(AriaRole.Combobox, new() { Name = "Status" });
                await status.SelectOptionAsync("confirmed");
                await page.GetByRole(AriaRole.Button, new() { Name = "Save", Exact = true })
                    .ClickAsync();
                await page.GetByRole(AriaRole.Heading, new() { Name = "Billable lines" })
                    .WaitForAsync();
                await page.ReloadAsync();

                (await status.InputValueAsync()).ShouldBe("confirmed");
            }
        );
    }
}
