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

        await page.ReloadAsync();
        await AssertPeriodsAsync(page, projectId);
        await AssertDocumentsAsync(page, projectId);
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
