using System.Reflection;
using System.Xml.Linq;

namespace Raccord.Architecture.Tests;

/// <summary>Executable versions of docs/architecture/architecture-rules.md.</summary>
public sealed class DependencyRulesTests
{
    private static readonly Assembly Domain = typeof(Raccord.Domain.Billing.InvoiceDraft).Assembly;
    private static readonly Assembly Application =
        typeof(Raccord.Application.Invoicing.InvoicingWorkflow).Assembly;
    private static readonly Assembly Contracts =
        typeof(Raccord.Contracts.Invoicing.IssuedInvoiceResponse).Assembly;
    private static readonly Assembly Infrastructure =
        typeof(Raccord.Infrastructure.DependencyInjection).Assembly;
    private static readonly Assembly SharedKernel =
        typeof(Raccord.SharedKernel.Common.Currency).Assembly;
    private static readonly Assembly BuildingBlocks =
        typeof(Raccord.BuildingBlocks.Api.ApiResults).Assembly;

    private static readonly string[] FrameworkPrefixes =
    [
        "System",
        "netstandard",
        "mscorlib",
        "Microsoft.Win32",
    ];

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
    public void Domain_Should_depend_only_on_the_base_class_library()
    {
        var offending = ReferencedNames(Domain)
            .Where(name =>
                !FrameworkPrefixes.Any(prefix => name.StartsWith(prefix, StringComparison.Ordinal))
                && !string.Equals(name, "Raccord.SharedKernel", StringComparison.Ordinal)
            );

        offending.ShouldBeEmpty();
    }

    [Fact]
    public void Contracts_Should_depend_only_on_the_base_class_library()
    {
        var offending = ReferencedNames(Contracts)
            .Where(name =>
                !FrameworkPrefixes.Any(prefix => name.StartsWith(prefix, StringComparison.Ordinal))
            );

        offending.ShouldBeEmpty();
    }

    [Fact]
    public void SharedKernel_Should_depend_only_on_the_base_class_library()
    {
        ReferencedNames(SharedKernel)
            .Where(name =>
                !FrameworkPrefixes.Any(prefix => name.StartsWith(prefix, StringComparison.Ordinal))
            )
            .ShouldBeEmpty();
    }

    [Fact]
    public void BuildingBlocks_Should_depend_only_on_shared_kernel_and_allowed_frameworks()
    {
        ReferencedNames(BuildingBlocks)
            .Where(name =>
                !FrameworkPrefixes.Any(prefix => name.StartsWith(prefix, StringComparison.Ordinal))
                && !name.StartsWith("Microsoft.", StringComparison.Ordinal)
                && !name.StartsWith("System.", StringComparison.Ordinal)
                && !name.StartsWith("Azure.", StringComparison.Ordinal)
                && !name.StartsWith("ErrorOr", StringComparison.Ordinal)
                && !name.StartsWith("FluentValidation", StringComparison.Ordinal)
                && !name.StartsWith("Npgsql", StringComparison.Ordinal)
                && !string.Equals(name, "Raccord.SharedKernel", StringComparison.Ordinal)
                && !string.Equals(name, "Raccord.Application", StringComparison.Ordinal)
            )
            .ShouldBeEmpty();
    }

    [Fact]
    public void Application_Should_have_no_web_database_cloud_or_infrastructure_dependencies()
    {
        var offending = ReferencedNames(Application)
            .Where(name =>
                ForbiddenInCore.Any(prefix => name.StartsWith(prefix, StringComparison.Ordinal))
            );

        offending.ShouldBeEmpty();
    }

    [Fact]
    public void Infrastructure_Should_not_depend_on_the_web_host()
    {
        ReferencedNames(Infrastructure)
            .ShouldNotContain(name => name.StartsWith("Raccord.Web", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("Raccord.Domain", new[] { "Raccord.SharedKernel" })]
    [InlineData("Raccord.Contracts", new string[0])]
    [InlineData(
        "Raccord.Application",
        new[]
        {
            "Raccord.BuildingBlocks",
            "Raccord.Contracts",
            "Raccord.Domain",
            "Raccord.Modules.Compliance.Contracts",
            "Raccord.SharedKernel",
        }
    )]
    [InlineData(
        "Raccord.Infrastructure",
        new[]
        {
            "Raccord.Application",
            "Raccord.BuildingBlocks",
            "Raccord.Contracts",
            "Raccord.Domain",
            "Raccord.Modules.Compliance",
            "Raccord.Modules.Compliance.Contracts",
            "Raccord.SharedKernel",
        }
    )]
    [InlineData(
        "Raccord.Web",
        new[]
        {
            "Raccord.Application",
            "Raccord.Contracts",
            "Raccord.Infrastructure",
            "Raccord.Modules.Compliance",
            "Raccord.BuildingBlocks",
            "Raccord.SharedKernel",
            "Raccord.ServiceDefaults",
        }
    )]
    [InlineData("Raccord.Aspire", new[] { "Raccord.Web" })]
    public void Project_references_Should_follow_the_dependency_direction(
        string project,
        string[] allowed
    )
    {
        var references = ProjectReferences(project);

        references.Order(StringComparer.Ordinal).ShouldBe(allowed.Order(StringComparer.Ordinal));
    }

    [Theory]
    [InlineData("Raccord.Domain")]
    [InlineData("Raccord.Contracts")]
    [InlineData("Raccord.Application")]
    public void Core_projects_Should_add_no_package_references(string project)
    {
        ProjectFile(project).Descendants("PackageReference").ShouldBeEmpty();
    }

    private static string[] ReferencedNames(Assembly assembly) =>
        [.. assembly.GetReferencedAssemblies().Select(reference => reference.Name ?? string.Empty)];

    private static IEnumerable<string> ProjectReferences(string project) =>
        ProjectFile(project)
            .Descendants("ProjectReference")
            .Select(reference =>
                Path.GetFileNameWithoutExtension(
                    ((string?)reference.Attribute("Include") ?? string.Empty).Replace('\\', '/')
                )
            );

    private static XDocument ProjectFile(string project)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (
            directory is not null && !File.Exists(Path.Combine(directory.FullName, "Raccord.slnx"))
        )
        {
            directory = directory.Parent;
        }

        directory.ShouldNotBeNull();
        return XDocument.Load(
            Path.Combine(directory.FullName, "src", project, project + ".csproj")
        );
    }
}
