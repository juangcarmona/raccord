using System.Reflection;
using Microsoft.Playwright;

namespace Raccord.EndToEndTests.Infrastructure;

/// <summary>
/// Where journey evidence lives: the absolute <c>PLAYWRIGHT_ARTIFACTS_DIR</c> when configured,
/// otherwise <c>playwright-artifacts/</c> relative to the runner's current directory. Success
/// screenshots and videos are opt-in; failure traces and screenshots remain best-effort so a
/// capture problem never replaces the journey's real failure.
/// </summary>
internal static class PlaywrightArtifacts
{
    internal const string Directory = "playwright-artifacts";
    internal static string EvidenceDirectory
    {
        get
        {
            var configuredDirectory = Environment.GetEnvironmentVariable(
                "PLAYWRIGHT_ARTIFACTS_DIR"
            );
            return Path.GetFullPath(
                string.IsNullOrWhiteSpace(configuredDirectory) ? Directory : configuredDirectory
            );
        }
    }
    internal static bool VisualEvidenceEnabled =>
        string.Equals(
            Environment.GetEnvironmentVariable("PLAYWRIGHT_CAPTURE_EVIDENCE"),
            "true",
            StringComparison.OrdinalIgnoreCase
        );

    internal static bool ShouldCaptureVideo(string evidenceMode, bool visualEvidenceEnabled) =>
        visualEvidenceEnabled
        && string.Equals(evidenceMode, E2ETraits.EvidenceModes.Browser, StringComparison.Ordinal);

    internal static string[] AreasFor(Type journeyType) =>
        journeyType
            .GetCustomAttributes(typeof(Xunit.TraitAttribute), inherit: false)
            .Cast<Xunit.TraitAttribute>()
            .Where(trait => string.Equals(trait.Name, E2ETraits.Area, StringComparison.Ordinal))
            .Select(trait => trait.Value)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

    internal static string EvidenceModeFor(Type journeyType)
    {
        var declaredMode = journeyType
            .GetCustomAttributes(typeof(Xunit.TraitAttribute), inherit: false)
            .Cast<Xunit.TraitAttribute>()
            .SingleOrDefault(trait =>
                string.Equals(trait.Name, E2ETraits.EvidenceMode, StringComparison.Ordinal)
            )
            ?.Value;

        if (declaredMode is null)
        {
            return E2ETraits.EvidenceModes.Browser;
        }

        if (string.Equals(declaredMode, E2ETraits.EvidenceModes.Api, StringComparison.Ordinal))
        {
            return E2ETraits.EvidenceModes.Api;
        }

        if (string.Equals(declaredMode, E2ETraits.EvidenceModes.Browser, StringComparison.Ordinal))
        {
            return E2ETraits.EvidenceModes.Browser;
        }

        throw new InvalidOperationException(
            $"E2E journey '{journeyType.Name}' declares unsupported evidence mode '{declaredMode}'."
        );
    }

    private static string ArtifactPath(string name) => Path.Combine(EvidenceDirectory, name);

    internal static void WriteJourneyResult(
        string name,
        string status,
        IReadOnlyCollection<string>? areas = null,
        IReadOnlyCollection<string>? supplementalAssets = null,
        string evidenceMode = E2ETraits.EvidenceModes.Browser
    )
    {
        try
        {
            System.IO.Directory.CreateDirectory(EvidenceDirectory);
            File.WriteAllText(
                ArtifactPath($"{name}.json"),
                System.Text.Json.JsonSerializer.Serialize(
                    new
                    {
                        name,
                        status,
                        viewport = "desktop",
                        areas = areas?.Distinct(StringComparer.Ordinal).ToArray() ?? [],
                        supplementalAssets = supplementalAssets
                            ?.Distinct(StringComparer.Ordinal)
                            .ToArray()
                            ?? [],
                        evidenceMode,
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
            System.IO.Directory.CreateDirectory(EvidenceDirectory);
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
        System.IO.Directory.CreateDirectory(EvidenceDirectory);
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

    /// <summary>Saves a failure trace for an API-only journey without capturing an unrelated page.</summary>
    internal static Task CaptureApiFailureAsync(IBrowserContext context, string name) =>
        SaveTraceAsync(context, $"{name}-failure");
}
