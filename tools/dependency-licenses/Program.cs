using System.Text.Json;

namespace Raccord.Tools.DependencyLicenses;

/// <summary>
/// Fails the build when a dependency carries a license outside the allowlist — the
/// load-bearing check behind Raccord's AGPL + brand-independence position (ADR-0002).
/// NuGet licenses live in nuspec metadata rather than in the compiled DLLs a container
/// image scan sees, so the graph is read from the restored projects'
/// <c>project.assets.json</c> plus the local nuspec cache, and the frontend's from
/// <c>package-lock.json</c>. SPDX expressions ("MIT OR Apache-2.0") are allowed when
/// every alternative is allowed.
/// </summary>
public static class Program
{
    /// <summary>
    /// Licenses cleared for distribution. NuGet license expressions and npm license
    /// strings both land here after expression parsing.
    /// </summary>
    private static readonly HashSet<string> AllowedLicenses =
    [
        "MIT",
        "MIT-0",
        "Apache-2.0",
        "BSD-2-Clause",
        "BSD-3-Clause",
        "BSD-3-Clause-Clear",
        "0BSD",
        "ISC",
        "BlueOak-1.0.0",
        "PostgreSQL",
        "Python-2.0",
        "MPL-2.0",
        "CC0-1.0",
        "CC-BY-4.0",
        "AGPL-3.0-only",
        "AGPL-3.0-or-later",
    ];

    /// <summary>
    /// Packages whose nuspec predates license expressions and only carries a license
    /// URL. Manually mapped after reading the linked license; keep this list short.
    /// </summary>
    private static readonly Dictionary<string, string> PreExpressionLicenses = new(
        StringComparer.OrdinalIgnoreCase
    )
    {
        // licenseUrl → dotnet/corefx LICENSE.TXT (MIT).
        ["System.Security.Cryptography.ProtectedData/4.5.0"] = "MIT",
        // The package predates nuspec license metadata; upstream repository license is MIT.
        ["ErrorOr/2.1.1"] = "MIT",
    };

    private static readonly string[] SPDXKeywords = ["OR", "AND", "WITH"];

    /// <summary>Entry point: parses <c>--check</c>, prints violations, returns the exit code.</summary>
    public static int Main(string[] args)
    {
        if (args.Length != 1 || !string.Equals(args[0], "--check", StringComparison.Ordinal))
        {
            Console.Error.WriteLine("usage: dependency-licenses --check");
            return 2;
        }

        var repoRoot = FindRepoRoot(AppContext.BaseDirectory);
        var violations = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

        violations.UnionWith(CheckNuGetGraph(repoRoot));
        violations.UnionWith(
            CheckNpmLockfile(Path.Combine(repoRoot, "src", "Raccord.Frontend", "package-lock.json"))
        );

        if (violations.Count == 0)
        {
            Console.WriteLine("License policy: OK (all dependency licenses allowed).");
            return 0;
        }

        Console.Error.WriteLine("License policy violations (license outside the allowlist):");
        foreach (var violation in violations)
        {
            Console.Error.WriteLine($"  {violation}");
        }

        return 1;
    }

    private static string FindRepoRoot(string start)
    {
        var dir = new DirectoryInfo(start);
        while (dir is not null && !dir.EnumerateFiles("Raccord.slnx").Any())
        {
            dir = dir.Parent;
        }

        return dir?.FullName
            ?? throw new InvalidOperationException("Could not locate the repository root.");
    }

    private static IEnumerable<string> CheckNuGetGraph(string repoRoot)
    {
        // The license talk is about what ships in a deliverable, so the runtime closure
        // (src/, not tests) is what gets checked — the union of every restored project
        // under src/ keeps that true as new projects appear.
        foreach (
            var assetsPath in Directory.EnumerateFiles(
                Path.Combine(repoRoot, "src"),
                "project.assets.json",
                SearchOption.AllDirectories
            )
        )
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(assetsPath));
            if (!doc.RootElement.TryGetProperty("libraries", out var libraries))
            {
                continue;
            }

            foreach (var library in libraries.EnumerateObject())
            {
                var packageRef = library.Name;
                // Project references ("Raccord.Domain/1.0.0") appear in the libraries
                // section too; only NuGet packages carry licenses.
                var isProject = library
                    .Value.EnumerateObject()
                    .Any(property =>
                        string.Equals(property.Name, "type", StringComparison.Ordinal)
                        && string.Equals(
                            property.Value.GetString(),
                            "project",
                            StringComparison.Ordinal
                        )
                    );
                if (isProject || !packageRef.Contains('/'))
                {
                    continue;
                }

                if (PreExpressionLicenses.TryGetValue(packageRef, out var mapped))
                {
                    if (!AllowedLicenses.Contains(mapped))
                    {
                        yield return $"{packageRef}: {mapped}";
                    }

                    continue;
                }

                var (packageId, version) = packageRef.Split('/') is { Length: 2 } parts
                    ? (parts[0], parts[1])
                    : throw new InvalidOperationException(
                        $"Malformed package reference '{packageRef}'."
                    );
                var license = ReadNuSpecLicense(packageId, version);
                if (license is null)
                {
                    yield return $"{packageRef}: no license expression in the nuspec (add to {nameof(PreExpressionLicenses)} after reading its licenseUrl)";
                    continue;
                }

                foreach (var disallowed in DisallowedTokens(license, packageRef))
                {
                    yield return disallowed;
                }
            }
        }
    }

    private static string? ReadNuSpecLicense(string packageId, string version)
    {
        // The nuspec cache layout: ~/.nuget/packages/<id>/<version>/<id>.nuspec, all
        // lowercase on disk regardless of the package's casing.
        var cacheRoot = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".nuget",
            "packages"
        );
        var nuspecPath = Path.Combine(
            cacheRoot,
            packageId.ToLowerInvariant(),
            version,
            $"{packageId.ToLowerInvariant()}.nuspec"
        );
        if (!File.Exists(nuspecPath))
        {
            return null;
        }

        // A nuspec is default-namespaced XML with a single <license> element; matching on
        // the local name avoids binding to the generated namespace. Some packages declare
        // an embedded license file instead of an expression — those map through
        // EmbeddedLicenses after reading the file once.
        var nuspec = System.Xml.Linq.XDocument.Load(nuspecPath);
        var license = nuspec
            .Descendants()
            .FirstOrDefault(element =>
                string.Equals(element.Name.LocalName, "license", StringComparison.Ordinal)
            );
        if (license is null)
        {
            return null;
        }

        if (string.Equals(license.Attribute("type")?.Value, "expression", StringComparison.Ordinal))
        {
            return license.Value.Trim();
        }

        if (string.Equals(license.Attribute("type")?.Value, "file", StringComparison.Ordinal))
        {
            return FileLicenses.TryGetValue(license.Value.Trim(), out var mapped) ? mapped : null;
        }

        return null;
    }

    /// <summary>
    /// Packages declaring an embedded license file rather than an expression. Mapped
    /// manually after reading the file; an unmapped file fails the check loudly.
    /// </summary>
    private static Dictionary<string, string> FileLicenses { get; } =
        new(StringComparer.OrdinalIgnoreCase)
        {
            // Fractions ships license.txt (MIT).
            ["license.txt"] = "MIT",
        };

    private static IEnumerable<string> CheckNpmLockfile(string lockfilePath)
    {
        if (!File.Exists(lockfilePath))
        {
            yield return $"{lockfilePath}: not found";
            yield break;
        }

        using var doc = JsonDocument.Parse(File.ReadAllText(lockfilePath));
        if (!doc.RootElement.TryGetProperty("packages", out var packages))
        {
            yield break;
        }

        foreach (var entry in packages.EnumerateObject())
        {
            if (!entry.Value.TryGetProperty("license", out var licenseElement))
            {
                continue; // the app's own entry and link entries carry no license
            }

            var license = licenseElement.GetString();
            if (string.IsNullOrWhiteSpace(license))
            {
                continue;
            }

            foreach (var disallowed in DisallowedTokens(license, entry.Name))
            {
                yield return disallowed;
            }
        }
    }

    private static IEnumerable<string> DisallowedTokens(string licenseExpression, string subject)
    {
        foreach (var token in licenseExpression.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            var cleaned = token.Trim('(', ')');
            if (cleaned.Length == 0 || SPDXKeywords.Contains(cleaned, StringComparer.Ordinal))
            {
                continue;
            }

            if (!AllowedLicenses.Contains(cleaned))
            {
                yield return $"{subject}: {licenseExpression} (token '{cleaned}' not allowed)";
            }
        }
    }
}
