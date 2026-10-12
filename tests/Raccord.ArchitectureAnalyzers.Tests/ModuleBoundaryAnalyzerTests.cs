using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Raccord.ArchitectureAnalyzers;
using Shouldly;

namespace Raccord.ArchitectureAnalyzers.Tests;

public sealed class ModuleBoundaryAnalyzerTests
{
    [Fact]
    public async Task Public_handler_Should_be_rejected()
    {
        var diagnostics = await AnalyzeAsync(
            "namespace Raccord.Modules.Contacts; public sealed class CustomerHandler { }"
        );

        diagnostics.ShouldContain(diagnostic => diagnostic.Id == "RACC001");
    }

    [Fact]
    public async Task Internal_sealed_handler_Should_be_allowed()
    {
        var diagnostics = await AnalyzeAsync(
            "namespace Raccord.Modules.Contacts; internal sealed class CustomerHandler { }"
        );

        diagnostics.ShouldNotContain(diagnostic => diagnostic.Id == "RACC001");
    }

    [Fact]
    public async Task Controller_Should_be_rejected()
    {
        var diagnostics = await AnalyzeAsync(
            "namespace Microsoft.AspNetCore.Mvc { public class ControllerBase { } }"
                + " namespace Raccord.Modules.Contacts; public class CustomerController : Microsoft.AspNetCore.Mvc.ControllerBase { }"
        );

        diagnostics.ShouldContain(diagnostic => diagnostic.Id == "RACC002");
    }

    [Fact]
    public async Task Public_member_exposing_persistence_type_Should_be_rejected()
    {
        var diagnostics = await AnalyzeAsync(
            "namespace Raccord.Modules.Contacts.Persistence { public class CustomerRow { } }"
                + " namespace Raccord.Modules.Contacts; public class CustomerQueries { public Raccord.Modules.Contacts.Persistence.CustomerRow Find() => null!; }"
        );

        diagnostics.ShouldContain(diagnostic => diagnostic.Id == "RACC003");
    }

    [Fact]
    public async Task Public_member_exposing_contract_type_Should_be_allowed()
    {
        var diagnostics = await AnalyzeAsync(
            "namespace Raccord.Modules.Contacts.Contracts { public record CustomerResponse; }"
                + " namespace Raccord.Modules.Contacts; public class CustomerQueries { public Raccord.Modules.Contacts.Contracts.CustomerResponse Find() => null!; }"
        );

        diagnostics.ShouldNotContain(diagnostic => diagnostic.Id == "RACC003");
    }

    [Fact]
    public async Task Domain_framework_using_Should_be_rejected()
    {
        var diagnostics = await AnalyzeAsync(
            "namespace Raccord.Modules.Contacts.Domain { using Microsoft.EntityFrameworkCore; public class Customer { } }"
        );

        diagnostics.ShouldContain(diagnostic => diagnostic.Id == "RACC004");
    }

    [Fact]
    public async Task Domain_bcl_using_Should_be_allowed()
    {
        var diagnostics = await AnalyzeAsync(
            "namespace Raccord.Modules.Contacts.Domain { using System; public class Customer { public DateTime CreatedAt { get; } } }"
        );

        diagnostics.ShouldNotContain(diagnostic => diagnostic.Id == "RACC004");
    }

    private static async Task<ImmutableArray<Diagnostic>> AnalyzeAsync(string source)
    {
        var tree = CSharpSyntaxTree.ParseText(source);
        var compilation = CSharpCompilation.Create(
            "AnalyzerTests",
            [tree],
            [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );
        var analyzers = ImmutableArray.Create<DiagnosticAnalyzer>(new ModuleBoundaryAnalyzer());
        var withAnalyzers = compilation.WithAnalyzers(analyzers);
        return await withAnalyzers.GetAnalyzerDiagnosticsAsync();
    }
}
