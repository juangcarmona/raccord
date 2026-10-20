using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Projects;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
public sealed class ProjectCancellationJourney_Should_PreserveInvoiceHistory(
    DistributedAppFixture app
) : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_WarnAboutOpenTasksAndKeepIssuedInvoiceAfterCancellation()
    {
        await RunJourneyAsync(
            "project-cancellation-invoice-history",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                var projectUrl = page.Url;

                await FinalizeDraftAsync(page, new DateOnly(2026, 9, 18));
                var invoiceUrl = page.Url;
                var projectId = new Uri(projectUrl).Segments[^1];
                await CreateOpenTaskAsync(page, projectId, RunStamp);

                await page.GotoAsync(projectUrl);
                await page.GetByRole(AriaRole.Button, new() { Name = "Cancel project" })
                    .ClickAsync();
                var previewWarning = page.GetByRole(AriaRole.Dialog).GetByRole(AriaRole.Alert);
                await previewWarning.WaitForAsync();
                (await previewWarning.InnerTextAsync()).ShouldBe(
                    "There are 1 open tasks, 0 assigned crew and 0 assigned vehicles. Cancellation will leave a persistent warning."
                );
                await page.GetByRole(AriaRole.Dialog)
                    .GetByRole(AriaRole.Button, new() { Name = "Confirm cancellation" })
                    .ClickAsync();
                await page.GetByRole(AriaRole.Dialog)
                    .WaitForAsync(new() { State = WaitForSelectorState.Hidden });

                await AssertCancellationWarningAsync(page);
                await page.ReloadAsync();
                await AssertCancellationWarningAsync(page);
                await page.GetByRole(AriaRole.Row)
                    .Filter(new() { HasText = "Camera kit with lenses" })
                    .GetByText("Invoiced")
                    .WaitForAsync();

                await page.GotoAsync(invoiceUrl);
                await page.GetByText("Issued and unalterable.").WaitForAsync();
                (await page.GetByText("Camera kit with lenses").CountAsync()).ShouldBeGreaterThan(
                    0
                );
            }
        );
    }

    private static async Task CreateOpenTaskAsync(IPage page, string projectId, string runStamp)
    {
        var statusResponse = await page.APIRequest.PostAsync(
            "/api/project-coordination/statuses",
            JsonOptions(
                new
                {
                    name = $"Open E2E {runStamp}",
                    position = 99,
                    completesProject = false,
                }
            )
        );
        statusResponse.Status.ShouldBe(201);
        using var statusJson = JsonDocument.Parse(await statusResponse.TextAsync());
        var taskResponse = await page.APIRequest.PostAsync(
            "/api/project-coordination/tasks",
            JsonOptions(
                new
                {
                    projectId,
                    title = "Confirm location access",
                    description = (string?)null,
                    statusId = statusJson.RootElement.GetProperty("id").GetGuid(),
                    position = 1,
                    assigneeId = (Guid?)null,
                }
            )
        );
        taskResponse.Status.ShouldBe(201);
    }

    private static async Task AssertCancellationWarningAsync(IPage page)
    {
        var warning = page.GetByRole(AriaRole.Alert);
        await warning.WaitForAsync();
        (await warning.InnerTextAsync()).ShouldBe(
            "This project was cancelled with open operational work."
        );
    }

    private static APIRequestContextOptions JsonOptions<T>(T value) => new() { DataObject = value };
}
