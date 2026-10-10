using System.Reflection;
using System.Xml.Linq;

namespace Raccord.Architecture.Tests;

/// <summary>Executable versions of docs/architecture/architecture-rules.md.</summary>
public sealed class DependencyRulesTests
{
    private static readonly Assembly Domain = typeof(Raccord.Domain.Billing.InvoiceDraft).Assembly;
    private static readonly Assembly Application = typeof(Raccord.Application.Invoicing.InvoicingWorkflow).Assembly;
    private static readonly Assembly Contracts = typeof(Raccord.Contracts.Invoicing.IssuedInvoiceResponse).Assembly;
    private static readonly Assembly Infrastructure = typeof(Raccord.Infrastructure.DependencyInjection).Assembly;

    private static readonly string[] FrameworkPrefixes = ["System", "netstandard", "mscorlib", "Microsoft.Win32"];

    private static readonly string[] ForbiddenInCore =
    [
        "Microsoft.AspNetCore",
        "Microsoft.EntityFrameworkCore",
        "Microsoft.Extensions.Hosting",
        "Npgsql",
        "Azure",
        "Aspire",
        "System.Net.Http",
        "Raccord.Infrastructure",
        "Raccord.Web",
    ];

    [Fact]
    public void Domain_depends_only_on_the_base_class_library()
    {
        var offending = ReferencedNames(Domain).Where(name => !FrameworkPrefixes.Any(prefix => name.StartsWith(prefix, StringComparison.Ordinal)));

        Assert.Empty(offending);
    }

    [Fact]
    public void Contracts_depend_only_on_the_base_class_library()
    {
        var offending = ReferencedNames(Contracts).Where(name => !FrameworkPrefixes.Any(prefix => name.StartsWith(prefix, StringComparison.Ordinal)));

        Assert.Empty(offending);
    }

    [Fact]
    public void Application_has_no_web_database_cloud_or_infrastructure_dependencies()
    {
        var offending = ReferencedNames(Application).Where(name => ForbiddenInCore.Any(prefix => name.StartsWith(prefix, StringComparison.Ordinal)));

        Assert.Empty(offending);
    }

    [Fact]
    public void Infrastructure_does_not_depend_on_the_web_host()
    {
        Assert.DoesNotContain(ReferencedNames(Infrastructure), name => name.StartsWith("Raccord.Web", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("Raccord.Domain", new string[0])]
    [InlineData("Raccord.Contracts", new string[0])]
    [InlineData("Raccord.Application", new[] { "Raccord.Contracts", "Raccord.Domain" })]
    [InlineData("Raccord.Infrastructure", new[] { "Raccord.Application", "Raccord.Contracts", "Raccord.Domain" })]
    [InlineData("Raccord.Web", new[] { "Raccord.Application", "Raccord.Contracts", "Raccord.Infrastructure", "Raccord.ServiceDefaults" })]
    [InlineData("Raccord.Aspire", new[] { "Raccord.Web" })]
    public void Project_references_follow_the_dependency_direction(string project, string[] allowed)
    {
        var references = ProjectReferences(project);

        Assert.Equal(allowed.Order(StringComparer.Ordinal), references.Order(StringComparer.Ordinal));
    }

    [Theory]
    [InlineData("Raccord.Domain")]
    [InlineData("Raccord.Contracts")]
    [InlineData("Raccord.Application")]
    public void Core_projects_add_no_package_references(string project)
    {
        Assert.Empty(ProjectFile(project).Descendants("PackageReference"));
    }

    private static string[] ReferencedNames(Assembly assembly) =>
        [.. assembly.GetReferencedAssemblies().Select(reference => reference.Name ?? string.Empty)];

    private static IEnumerable<string> ProjectReferences(string project) =>
        ProjectFile(project)
            .Descendants("ProjectReference")
            .Select(reference => Path.GetFileNameWithoutExtension(((string?)reference.Attribute("Include") ?? string.Empty).Replace('\\', '/')));

    private static XDocument ProjectFile(string project)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "Raccord.slnx")))
        {
            directory = directory.Parent;
        }

        Assert.NotNull(directory);
        return XDocument.Load(Path.Combine(directory.FullName, "src", project, project + ".csproj"));
    }
}
