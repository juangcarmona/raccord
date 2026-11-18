using System.Net;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Workspace;

[Trait(E2ETraits.Area, E2ETraits.Areas.Workspace)]
[Collection(FreshInstallationCollection.Name)]
public sealed class FirstRunSetupJourney_Should_CreateAdministrator(FreshInstallationFixture app)
{
    private static readonly string[] ExpectedRoles = ["Admin", "Crew", "Office", "Warehouse"];

    [Fact]
    public async Task Journey_Should_CreateTheFirstAdministratorAndRefuseARepeat()
    {
        // No trace/video: request bodies and DOM snapshots must not retain credentials.
        await using var context = await app.NewContextAsync(captureVideo: false);
        var page = await context.NewPageAsync();
        Assert.Equal(0, await app.CountSetupRecordsAsync("users"));
        var status = await page.APIRequest.GetAsync(
            new Uri(app.WebBaseUrl, "/api/setup/status").AbsoluteUri
        );
        Assert.True(status.Ok);
        Assert.True((await status.JsonAsync())!.Value.GetProperty("required").GetBoolean());
        await page.GotoAsync("/");
        await page.GetByRole(AriaRole.Heading, new() { Name = "Create the first administrator" })
            .WaitForAsync();
        await CaptureAppearancesAsync(page);
        var stale = await context.NewPageAsync();
        await stale.GotoAsync("/setup");
        await stale.GetByLabel("Email").WaitForAsync();
        await page.GetByLabel("Email").FillAsync("first@example.test");
        await page.GetByLabel("Password").FillAsync("SyntheticPassword123!");
        await page.GetByRole(AriaRole.Button, new() { Name = "Create administrator account" })
            .ClickAsync();
        await page.GetByText("The administrator account was created.", new() { Exact = true })
            .WaitForAsync();
        Assert.Equal(1, await app.CountSetupRecordsAsync("users"));
        Assert.Equal(4, await app.CountSetupRecordsAsync("roles"));
        Assert.Equal(1, await app.CountSetupRecordsAsync("user_roles"));

        await stale.GetByLabel("Email").FillAsync("another@example.test");
        await stale.GetByLabel("Password").FillAsync("SyntheticPassword123!");
        await stale
            .GetByRole(AriaRole.Button, new() { Name = "Create administrator account" })
            .ClickAsync();
        await stale
            .GetByRole(AriaRole.Alert)
            .GetByText("An account already exists in this installation. Sign in instead.")
            .WaitForAsync();
        Assert.Equal(
            0,
            await stale
                .GetByRole(AriaRole.Button, new() { Name = "Create administrator account" })
                .CountAsync()
        );
        await page.GetByRole(AriaRole.Button, new() { Name = "Continue to the application" })
            .ClickAsync();
        // The first account retains its existing Spanish profile default.
        await page.WaitForURLAsync("**/projects");
        await VerifyRoleAndRepeatRefusalAsync(page);
        await VerifyCompletedSetupRoutesAsync(stale);
        PlaywrightArtifacts.WriteJourneyResult(
            "first-run-setup",
            "passed",
            [E2ETraits.Areas.Workspace]
        );
    }

    private async Task VerifyRoleAndRepeatRefusalAsync(IPage page)
    {
        var roles = await page.APIRequest.GetAsync("/api/roles");
        Assert.True(roles.Ok);
        var roleData = (await roles.JsonAsync())!.Value;
        Assert.Equal(
            ExpectedRoles,
            roleData.EnumerateArray().Select(role => role.GetProperty("name").GetString()).ToArray()
        );
        Assert.Equal(
            1,
            roleData
                .EnumerateArray()
                .Single(role =>
                    string.Equals(
                        role.GetProperty("name").GetString(),
                        "Admin",
                        StringComparison.Ordinal
                    )
                )
                .GetProperty("userCount")
                .GetInt32()
        );

        var attempts = await Task.WhenAll(
            Enumerable
                .Range(1, 3)
                .Select(index =>
                    page.APIRequest.PostAsync(
                        "/api/setup",
                        new()
                        {
                            DataObject = new
                            {
                                email = $"repeat-{index}@example.test",
                                password = "SyntheticPassword123!",
                            },
                        }
                    )
                )
        );
        Assert.All(
            attempts,
            response => Assert.Equal((int)HttpStatusCode.Conflict, response.Status)
        );
        Assert.Equal(1, await app.CountSetupRecordsAsync("users"));
        Assert.Equal(4, await app.CountSetupRecordsAsync("roles"));
    }

    private async Task VerifyCompletedSetupRoutesAsync(IPage signedInPage)
    {
        await signedInPage.GotoAsync("/setup");
        await signedInPage.WaitForURLAsync("**/projects");
        await using var signedOut = await app.NewContextAsync(captureVideo: false);
        var page = await signedOut.NewPageAsync();
        await page.GotoAsync("/setup");
        await page.WaitForURLAsync("**/sign-in");
    }

    private static async Task CaptureAppearancesAsync(IPage page)
    {
        Directory.CreateDirectory(PlaywrightArtifacts.EvidenceDirectory);
        foreach (var width in new[] { 1280, 375 })
        {
            await page.SetViewportSizeAsync(width, 812);
            foreach (var theme in new[] { ColorScheme.Light, ColorScheme.Dark })
            {
                await page.EmulateMediaAsync(
                    new() { ColorScheme = theme, ReducedMotion = ReducedMotion.Reduce }
                );
                Assert.False(
                    await page.EvaluateAsync<bool>(
                        "() => document.documentElement.scrollWidth > innerWidth"
                    )
                );
                await page.GetByLabel("Email").FocusAsync();
                Assert.NotEqual(
                    "none",
                    await page.EvaluateAsync<string>(
                        "() => getComputedStyle(document.activeElement).outlineStyle"
                    )
                );
                await page.ScreenshotAsync(
                    new()
                    {
                        Path = Path.Combine(
                            PlaywrightArtifacts.EvidenceDirectory,
                            $"first-run-setup-{width}-{theme}.png"
                        ),
                        FullPage = false,
                        Mask = [page.Locator("input")],
                    }
                );
            }
        }
        await page.SetViewportSizeAsync(1280, 720);
    }
}
