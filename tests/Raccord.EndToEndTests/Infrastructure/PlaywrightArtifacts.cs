using Microsoft.Playwright;

namespace Raccord.EndToEndTests.Infrastructure;

/// <summary>
/// Where failed-journey evidence lives: <c>playwright-artifacts/</c> under the test output
/// directory, gitignored in the repo root. Kept tiny and best-effort on purpose — a capture
/// failure must not replace the journey's real failure.
/// </summary>
internal static class PlaywrightArtifacts
{
    internal const string Directory = "playwright-artifacts";

    /// <summary>
    /// Saves the context's trace (<c>{name}.zip</c>) — the browser-actions timeline, screenshots
    /// and DOM snapshots Playwright records while tracing is on. The CI job uploads this on failure.
    /// </summary>
    internal static async Task SaveTraceAsync(IBrowserContext context, string name)
    {
        try
        {
            await context.Tracing.StopAsync(
                new() { Path = Path.Combine(Directory, $"{name}.zip") }
            );
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
            await context.Tracing.StopAsync(
                new() { Path = Path.Combine(Directory, $"{name}-failure.zip") }
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Saving failure trace for '{name}' failed: {ex.Message}");
        }

        try
        {
            await page.ScreenshotAsync(
                new() { Path = Path.Combine(Directory, $"{name}-failure.png"), FullPage = true }
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Saving failure screenshot for '{name}' failed: {ex.Message}");
        }
    }
}
