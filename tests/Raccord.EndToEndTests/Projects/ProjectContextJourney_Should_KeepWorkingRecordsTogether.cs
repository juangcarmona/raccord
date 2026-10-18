using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Projects;

/// <summary>
/// JRN-PRJ-CONTEXT / UC-PRJ-ADD-NOTE / UC-PRJ-ATTACH-FILE / UC-PRJ-MANAGE-FILE-FOLDERS / UC-PRJ-DEFINE-CUSTOM-FIELD / UC-PRJ-SAVE-VIEW / UC-PRJ-TAG-RECORDS / UC-PRJ-REVIEW-HISTORY.
/// Given a project, when an operator adds working records and retrieves its context, then notes, files, folders, fields, tags, views and audit history remain together.
/// ProductShape: BC-PROJECTS, JRN-PRJ-CREATE-PROJECT, UC-PRJ-COORDINATE-PROJECT.
/// </summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
public sealed class ProjectContextJourney_Should_KeepWorkingRecordsTogether(
    DistributedAppFixture app
) : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_KeepProjectContextAndHistoryTogether()
    {
        await RunJourneyAsync(
            "project-context",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                var projectId = new Uri(page.Url).Segments[^1];

                await PostAsync(
                    page,
                    $"/api/projects/{projectId}/context/notes",
                    "{\"title\":\"Call sheet\",\"body\":\"Load-in at 08:00\",\"actor\":\"e2e\"}"
                );
                await PostAsync(
                    page,
                    $"/api/projects/{projectId}/context/folders",
                    "{\"name\":\"Production\",\"actor\":\"e2e\"}"
                );
                await PostAsync(
                    page,
                    $"/api/projects/{projectId}/context/files",
                    "{\"name\":\"call-sheet.txt\",\"folder\":\"Production\",\"content\":\"Crew arrival\",\"actor\":\"e2e\"}"
                );
                await PostAsync(
                    page,
                    $"/api/projects/{projectId}/context/fields",
                    "{\"key\":\"venue\",\"label\":\"Venue\",\"value\":\"Teatro E2E\",\"actor\":\"e2e\"}"
                );
                await PostAsync(
                    page,
                    $"/api/projects/{projectId}/context/tags",
                    "{\"tag\":\"priority\",\"recordType\":\"project\",\"actor\":\"e2e\"}"
                );
                await PostAsync(
                    page,
                    $"/api/projects/{projectId}/context/views",
                    "{\"name\":\"Production view\",\"definition\":\"{\\\"status\\\":\\\"active\\\"}\",\"actor\":\"e2e\"}"
                );

                var response = await page.APIRequest.GetAsync($"/api/projects/{projectId}/context");
                response.Ok.ShouldBeTrue();
                using var document = JsonDocument.Parse(await response.TextAsync());
                var entries = document.RootElement.GetProperty("entries");
                entries.GetArrayLength().ShouldBe(6);
                entries
                    .EnumerateArray()
                    .Select(item => item.GetProperty("kind").GetString())
                    .ShouldBe(
                        ["note", "folder", "file", "field", "tag", "view"],
                        StringComparer.Ordinal
                    );
                document.RootElement.GetProperty("history").GetArrayLength().ShouldBe(6);
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
