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
                var projectUrl = page.Url;

                await page.GotoAsync("/customers/new");
                await page.GetByLabel("Legal name").FillAsync($"Alternative E2E {RunStamp}");
                await page.GetByLabel("Tax ID").FillAsync("B87654322");
                await page.GetByLabel("Billing e-mail")
                    .FillAsync("alternative@productora-e2e.example");
                await page.GetByLabel("Street and number").FillAsync("Calle de pruebas 2");
                await page.GetByLabel("Postal code").FillAsync("28002");
                await page.GetByLabel("City").FillAsync("Madrid");
                await page.GetByLabel("Country (ISO").FillAsync("ES");
                await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
                await page.WaitForURLAsync(url =>
                    url.EndsWith("/customers", StringComparison.Ordinal)
                );
                await page.GotoAsync(projectUrl);

                await page.GetByLabel("Project name").FillAsync("Summer campaign shoot");
                await page.GetByLabel("Customer")
                    .SelectOptionAsync(
                        new SelectOptionValue { Label = $"Alternative E2E {RunStamp}" }
                    );
                await page.GetByLabel("Start").FillAsync("2026-10-01");
                await page.GetByLabel("End").FillAsync("2026-10-03");
                await page.GetByRole(AriaRole.Button, new() { Name = "Save details" }).ClickAsync();
                await page.GetByRole(AriaRole.Heading, new() { Name = "Summer campaign shoot" })
                    .WaitForAsync();
                await page.ReloadAsync();

                (await page.GetByLabel("Project name").InputValueAsync()).ShouldBe(
                    "Summer campaign shoot"
                );
                (
                    await page.GetByLabel("Customer").Locator("option:checked").TextContentAsync()
                ).ShouldBe($"Alternative E2E {RunStamp}");
                (await page.GetByLabel("Start").InputValueAsync()).ShouldBe("2026-10-01");
                (await page.GetByLabel("End").InputValueAsync()).ShouldBe("2026-10-03");
            }
        );
    }
}
