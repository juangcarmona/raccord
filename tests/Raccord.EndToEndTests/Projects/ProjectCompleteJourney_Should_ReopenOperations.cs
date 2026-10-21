using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Projects;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
[Trait(E2ETraits.Rule, "complete-read-only")]
public sealed class ProjectCompleteJourney_Should_ReopenOperations(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_BlockOperationsUntilTheProjectIsReopened()
    {
        await RunJourneyAsync(
            "project-complete-reopen",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                var status = page.GetByRole(AriaRole.Combobox, new() { Name = "Status" });
                var statusPanel = page.Locator("section[aria-labelledby='project-status-heading']");

                await status.SelectOptionAsync("complete");
                await statusPanel
                    .GetByRole(AriaRole.Button, new() { Name = "Save", Exact = true })
                    .ClickAsync();
                await page.ReloadAsync();
                (await page.GetByLabel("Project name").IsDisabledAsync()).ShouldBeTrue();
                (
                    await page.GetByRole(AriaRole.Button, new() { Name = "Add period" })
                        .IsDisabledAsync()
                ).ShouldBeTrue();
                (
                    await page.GetByRole(AriaRole.Heading, new() { Name = "Add billable line" })
                        .CountAsync()
                ).ShouldBe(0);

                await status.SelectOptionAsync("in-progress");
                await statusPanel
                    .GetByRole(AriaRole.Button, new() { Name = "Save", Exact = true })
                    .ClickAsync();
                await page.ReloadAsync();
                (await page.GetByLabel("Project name").IsDisabledAsync()).ShouldBeFalse();
                await page.GetByLabel("New period name").FillAsync("Show");
                await page.GetByRole(AriaRole.Button, new() { Name = "Add period" }).ClickAsync();
                await page.GetByRole(AriaRole.Form, new() { Name = "Period Show" }).WaitForAsync();
            }
        );
    }
}
