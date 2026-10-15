using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Projects;

/// <summary>
/// JRN-PRJ-TIME / UC-PRJ-TRACK-TASK-TIME / UC-PRJ-REPORT-TASK-TIME / UC-PRJ-MANAGE-SERVICES.
/// Given a project, when an operator records planned and actual service time, then the report keeps both source values and derives their variance.
/// ProductShape: BC-PROJECTS, JRN-PRJ-COORDINATE-PROJECT, UC-PRJ-COORDINATE-PROJECT.
/// </summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
public sealed class ProjectTimeJourney_Should_ReportPlannedAndActualWork(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_ReportPlannedAndActualTimeWithoutChangingSourceRecords()
    {
        await RunJourneyAsync(
            "project-time-report",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                var projectId = new Uri(page.Url).Segments[^1];

                await PostAsync(
                    page,
                    $"/api/projects/{projectId}/time",
                    "{\"service\":\"Load-in\",\"plannedHours\":8,\"actualHours\":10,\"actor\":\"e2e\"}"
                );
                await PostAsync(
                    page,
                    $"/api/projects/{projectId}/time",
                    "{\"service\":\"Show call\",\"plannedHours\":6.5,\"actualHours\":6,\"actor\":\"e2e\"}"
                );

                var report = await page.APIRequest.GetAsync(
                    $"/api/projects/{projectId}/time-report"
                );
                report.Ok.ShouldBeTrue();
                using var document = JsonDocument.Parse(await report.TextAsync());
                document.RootElement.GetProperty("plannedHours").GetDecimal().ShouldBe(14.5m);
                document.RootElement.GetProperty("actualHours").GetDecimal().ShouldBe(16m);
                document.RootElement.GetProperty("varianceHours").GetDecimal().ShouldBe(1.5m);
                document.RootElement.GetProperty("entries").GetArrayLength().ShouldBe(2);
            }
        );
    }

    private static async Task PostAsync(IPage page, string path, string json)
    {
        var response = await page.APIRequest.PostAsync(
            path,
            new APIRequestContextOptions
            {
                Data = json,
                Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["content-type"] = "application/json",
                },
            }
        );
        response.Status.ShouldBe(201);
    }
}
