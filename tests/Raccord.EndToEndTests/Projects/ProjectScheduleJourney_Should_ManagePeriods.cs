using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Projects;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
public sealed class ProjectScheduleJourney_Should_ManagePeriods(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_EditOrderAndRenderNamedPeriodsByAudience()
    {
        await RunJourneyAsync(
            "project-time-schedule",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                await ManagePeriodsAsync(page, new Uri(page.Url).Segments[^1]);
            }
        );
    }

    private static async Task ManagePeriodsAsync(IPage page, string projectId)
    {
        await page.GetByLabel("New period name").FillAsync("Load-in");
        await page.GetByRole(AriaRole.Button, new() { Name = "Add period" }).ClickAsync();
        var period = page.GetByRole(AriaRole.Form, new() { Name = "Period Load-in" });
        await period.GetByLabel("Period name").FillAsync("Build");
        await period.GetByLabel("Period start").FillAsync("2026-05-04T08:00");
        await period.GetByLabel("Period end").FillAsync("2026-05-04T18:00");
        await period.GetByLabel("Show on internal documents").UncheckAsync();
        await period.GetByLabel("Show on external documents").CheckAsync();
        await period.GetByRole(AriaRole.Button, new() { Name = "Save", Exact = true }).ClickAsync();

        period = page.GetByRole(AriaRole.Form, new() { Name = "Period Build" });
        var designationResponse = page.WaitForResponseAsync(response =>
            response.Url.Contains("/designation", StringComparison.Ordinal) && response.Ok
        );
        await period
            .GetByRole(AriaRole.Button, new() { Name = "Use as planning period" })
            .ClickAsync();
        await designationResponse;
        await AssertInvalidUsageRangeAsync(page, projectId);
        var reorderResponse = page.WaitForResponseAsync(response =>
            response.Url.Contains("/periods/order", StringComparison.Ordinal) && response.Ok
        );
        await period.GetByRole(AriaRole.Button, new() { Name = "Move period up" }).ClickAsync();
        await reorderResponse;
        period = page.GetByRole(AriaRole.Form, new() { Name = "Period Build" });
        var duplicateResponse = page.WaitForResponseAsync(response =>
            response.Url.Contains("/duplicate", StringComparison.Ordinal) && response.Ok
        );
        await period.GetByRole(AriaRole.Button, new() { Name = "Duplicate period" }).ClickAsync();
        await duplicateResponse;

        var buildPeriodId = await GenerateAndMoveFunctionsAsync(page, projectId);

        await page.ReloadAsync();
        await AssertPeriodsAsync(page, projectId);
        await AssertGeneratedFunctionsAsync(page, projectId);
        await AssertFunctionPropagationAsync(page, projectId, buildPeriodId);
        await AssertDocumentsAsync(page, projectId);
    }

    private static async Task<string> GenerateAndMoveFunctionsAsync(IPage page, string projectId)
    {
        await page.GetByLabel("Select Build to create functions", new() { Exact = true })
            .CheckAsync();
        await page.GetByLabel("Select Build copy to create functions", new() { Exact = true })
            .CheckAsync();
        await page.GetByLabel("Crew functions", new() { Exact = true }).CheckAsync();
        await page.GetByLabel("Transport functions", new() { Exact = true }).CheckAsync();
        var generationResponse = page.WaitForResponseAsync(response =>
            response.Url.Contains("/functions/from-periods", StringComparison.Ordinal)
            && response.Ok
        );
        await page.GetByRole(AriaRole.Button, new() { Name = "Create selected functions" })
            .ClickAsync();
        await generationResponse;

        var periodsBeforeMove = await page.APIRequest.GetAsync(
            $"/api/projects/{projectId}/periods"
        );
        using var periodBody = JsonDocument.Parse(await periodsBeforeMove.TextAsync());
        var buildPeriod = periodBody
            .RootElement.EnumerateArray()
            .Single(item =>
                string.Equals(
                    item.GetProperty("name").GetString(),
                    "Build",
                    StringComparison.Ordinal
                )
            );
        var buildPeriodId = buildPeriod.GetProperty("id").GetString()!;
        var unlinkedResponse = await page.APIRequest.PostAsync(
            $"/api/projects/{projectId}/functions",
            new()
            {
                DataObject = new
                {
                    name = "Independent crew",
                    kind = 0,
                    startDate = "2026-05-04",
                    endDate = "2026-05-04",
                    requiredResources = 1,
                },
            }
        );
        unlinkedResponse.Status.ShouldBe(201);
        var buildForm = page.GetByRole(
            AriaRole.Form,
            new() { Name = "Period Build", Exact = true }
        );
        await buildForm.GetByLabel("Period start").FillAsync("2026-05-05T08:00");
        await buildForm.GetByLabel("Period end").FillAsync("2026-05-05T18:00");
        var moveResponse = page.WaitForResponseAsync(response =>
            response.Url.Contains($"/periods/{buildPeriodId}", StringComparison.Ordinal)
            && string.Equals(response.Request.Method, "PUT", StringComparison.Ordinal)
            && response.Ok
        );
        await buildForm
            .GetByRole(AriaRole.Button, new() { Name = "Save", Exact = true })
            .ClickAsync();
        await moveResponse;
        return buildPeriodId;
    }

    private static async Task AssertGeneratedFunctionsAsync(IPage page, string projectId)
    {
        var response = await page.APIRequest.GetAsync($"/api/projects/{projectId}/functions");
        response.Status.ShouldBe(200);
        using var functions = JsonDocument.Parse(await response.TextAsync());
        var generated = functions
            .RootElement.EnumerateArray()
            .Where(item =>
                item.TryGetProperty("projectTimePeriodId", out var id)
                && id.ValueKind == JsonValueKind.String
            )
            .ToArray();
        generated.Length.ShouldBe(4);
        generated
            .Select(item => item.GetProperty("projectTimePeriodId").GetString())
            .Distinct(StringComparer.Ordinal)
            .Count()
            .ShouldBe(2);
        generated
            .Select(item => item.GetProperty("kind").GetInt32())
            .Distinct()
            .Count()
            .ShouldBe(2);
    }

    private static async Task AssertFunctionPropagationAsync(
        IPage page,
        string projectId,
        string linkedPeriodId
    )
    {
        var response = await page.APIRequest.GetAsync($"/api/projects/{projectId}/functions");
        using var functions = JsonDocument.Parse(await response.TextAsync());
        var linked = functions
            .RootElement.EnumerateArray()
            .Where(item =>
                item.TryGetProperty("projectTimePeriodId", out var periodId)
                && string.Equals(periodId.GetString(), linkedPeriodId, StringComparison.Ordinal)
            )
            .ToArray();
        linked.Length.ShouldBe(2);
        linked.ShouldAllBe(item =>
            string.Equals(
                item.GetProperty("startDate").GetString(),
                "2026-05-05",
                StringComparison.Ordinal
            )
        );
        var unlinked = functions
            .RootElement.EnumerateArray()
            .Single(item =>
                string.Equals(
                    item.GetProperty("name").GetString(),
                    "Independent crew",
                    StringComparison.Ordinal
                )
            );
        unlinked.GetProperty("startDate").GetString().ShouldBe("2026-05-04");
        unlinked.GetProperty("projectTimePeriodId").ValueKind.ShouldBe(JsonValueKind.Null);
    }

    private static async Task AssertInvalidUsageRangeAsync(IPage page, string projectId)
    {
        var usage = page.GetByRole(AriaRole.Form, new() { Name = "Period Usage" });
        await usage.GetByLabel("Period start").FillAsync("2026-05-04T07:00");
        await usage.GetByLabel("Period end").FillAsync("2026-05-04T19:00");
        var updateResponse = page.WaitForResponseAsync(response =>
            response.Url.Contains("/periods/", StringComparison.Ordinal)
            && string.Equals(response.Request.Method, "PUT", StringComparison.Ordinal)
            && response.Status == 422
        );
        await usage.GetByRole(AriaRole.Button, new() { Name = "Save", Exact = true }).ClickAsync();
        await updateResponse;
        await page.GetByRole(AriaRole.Alert).WaitForAsync();
        var persisted = await page.APIRequest.GetAsync($"/api/projects/{projectId}/periods");
        using var periods = JsonDocument.Parse(await persisted.TextAsync());
        periods
            .RootElement.EnumerateArray()
            .Single(item => item.GetProperty("isDefaultUsage").GetBoolean())
            .GetProperty("start")
            .ValueKind.ShouldBe(JsonValueKind.Null);
    }

    private static async Task AssertPeriodsAsync(IPage page, string projectId)
    {
        var response = await page.APIRequest.GetAsync($"/api/projects/{projectId}/periods");
        response.Status.ShouldBe(200);
        using var periods = JsonDocument.Parse(await response.TextAsync());
        var orderedNames = periods
            .RootElement.EnumerateArray()
            .Select(item => item.GetProperty("name").GetString())
            .ToArray();
        orderedNames.ShouldBe(["Planning", "Build", "Build copy", "Usage"]);
        periods.RootElement[1].GetProperty("isDefaultPlanning").GetBoolean().ShouldBeTrue();
        periods.RootElement[1].GetProperty("showOnInternalDocuments").GetBoolean().ShouldBeFalse();
        periods.RootElement[1].GetProperty("showOnExternalDocuments").GetBoolean().ShouldBeTrue();
    }

    private static async Task AssertDocumentsAsync(IPage page, string projectId)
    {
        var internalDocument = await page.APIRequest.GetAsync(
            $"/api/projects/{projectId}/document?audience=internal"
        );
        var externalDocument = await page.APIRequest.GetAsync(
            $"/api/projects/{projectId}/document?audience=external"
        );
        internalDocument.Status.ShouldBe(200);
        externalDocument.Status.ShouldBe(200);
        using var internalBody = JsonDocument.Parse(await internalDocument.TextAsync());
        using var externalBody = JsonDocument.Parse(await externalDocument.TextAsync());
        var internalHtml = internalBody.RootElement.GetProperty("html").GetString() ?? "";
        var externalHtml = externalBody.RootElement.GetProperty("html").GetString() ?? "";
        internalHtml.ShouldNotContain("Build");
        externalHtml.ShouldContain("Build");
    }
}
