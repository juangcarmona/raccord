using Microsoft.Playwright;

namespace Raccord.EndToEndTests.Infrastructure;

/// <summary>
/// Where journey evidence lives: <c>playwright-artifacts/</c> under the test output directory,
/// gitignored in the repo root. Success screenshots and videos are opt-in; failure traces and
/// screenshots remain best-effort so a capture problem never replaces the journey's real failure.
/// </summary>
internal static class PlaywrightArtifacts
{
    internal const string Directory = "playwright-artifacts";
    internal static bool VisualEvidenceEnabled =>
        string.Equals(
            Environment.GetEnvironmentVariable("PLAYWRIGHT_CAPTURE_EVIDENCE"),
            "true",
            StringComparison.OrdinalIgnoreCase
        );

    private static string ArtifactPath(string name) => Path.Combine(Directory, name);

    internal static void WriteJourneyResult(string name, string status)
    {
        try
        {
            System.IO.Directory.CreateDirectory(Directory);
            File.WriteAllText(
                ArtifactPath($"{name}.json"),
                System.Text.Json.JsonSerializer.Serialize(
                    new
                    {
                        name,
                        status,
                        viewport = "desktop",
                    }
                )
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Saving result for '{name}' failed: {ex.Message}");
        }
    }

    internal static async Task CaptureSuccessAsync(IPage page, string name)
    {
        try
        {
            System.IO.Directory.CreateDirectory(Directory);
            await page.ScreenshotAsync(
                new() { Path = ArtifactPath($"{name}-checkpoint.png"), FullPage = true }
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Saving success screenshot for '{name}' failed: {ex.Message}");
        }
    }

    internal static async Task SaveVideoAsync(IPage page, string name)
    {
        try
        {
            var video = page.Video;
            if (video is not null)
            {
                await video.SaveAsAsync(ArtifactPath($"{name}.webm"));
                await video.DeleteAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Saving video for '{name}' failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Saves the context's trace (<c>{name}.zip</c>) — the browser-actions timeline, screenshots
    /// and DOM snapshots Playwright records while tracing is on. The CI job uploads this on failure.
    /// </summary>
    internal static async Task SaveTraceAsync(IBrowserContext context, string name)
    {
        try
        {
            await context.Tracing.StopAsync(new() { Path = ArtifactPath($"{name}.zip") });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Saving trace for '{name}' failed: {ex.Message}");
        }
    }

    /// <summary>
    /// On a failed journey, stops tracing into <c>{name}-failure.zip</c> (including everything
    /// recorded so far) and adds a screenshot of the page in the state the failure left it in.
    /// </summary>
    internal static async Task CaptureAsync(IBrowserContext context, IPage page, string name)
    {
        System.IO.Directory.CreateDirectory(Directory);
        try
        {
            await context.Tracing.StopAsync(new() { Path = ArtifactPath($"{name}-failure.zip") });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Saving failure trace for '{name}' failed: {ex.Message}");
        }

        try
        {
            await page.ScreenshotAsync(
                new() { Path = ArtifactPath($"{name}-failure.png"), FullPage = true }
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Saving failure screenshot for '{name}' failed: {ex.Message}");
        }
    }
}
