using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Projects;

/// <summary>
/// Creates a project for a freshly registered customer and adds a billable line to it — the
/// project leg of the smoke set, and the setup leg the invoice journey reuses.
/// </summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
public sealed class ProjectJourney_Should_AcceptBillableLine(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_CreateAProjectAndListItsBillableLine()
    {
        await RunJourneyAsync(
            "project-billable-line",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);

                // The line appears unbilled — billing state is the domain behaviour this journey
                // guards, beyond the row merely rendering.
                await page.GetByRole(AriaRole.Row)
                    .Filter(new() { HasText = "Camera kit with lenses" })
                    .GetByText("Unbilled")
                    .WaitForAsync();
            }
        );
    }
}
