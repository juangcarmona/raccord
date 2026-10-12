using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Raccord.ArchitectureAnalyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ModuleBoundaryAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor HandlerVisibility = new(
        "RACC001",
        "Handlers must be internal sealed",
        "Handler '{0}' must be declared internal sealed",
        "Architecture",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor ControllerForbidden = new(
        "RACC002",
        "MVC controllers are forbidden in modules",
        "Module type '{0}' must not derive from MVC controller '{1}'",
        "Architecture",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor PublicPersistenceType = new(
        "RACC003",
        "Public members must not expose persistence types",
        "Public member '{0}' exposes persistence or domain type '{1}'",
        "Architecture",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor DomainFrameworkReference = new(
        "RACC004",
        "Domain code must be framework-free",
        "Domain code must not reference framework namespace '{0}'",
        "Architecture",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly string[] ForbiddenDomainPrefixes =
    [
        "Microsoft.EntityFrameworkCore",
        "Microsoft.AspNetCore",
        "Microsoft.Extensions",
        "Npgsql",
        "Azure.",
        "Aspire.",
        "ErrorOr",
        "FluentValidation",
    ];

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        [HandlerVisibility, ControllerForbidden, PublicPersistenceType, DomainFrameworkReference];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
        context.RegisterSyntaxNodeAction(AnalyzeUsing, SyntaxKind.UsingDirective);
    }

    private static void AnalyzeClass(SyntaxNodeAnalysisContext context)
    {
        var declaration = (ClassDeclarationSyntax)context.Node;
        var symbol = context.SemanticModel.GetDeclaredSymbol(declaration);
        if (symbol is null || !IsModuleType(symbol.ContainingNamespace))
        {
            return;
        }

        if (symbol.Name.EndsWith("Handler", StringComparison.Ordinal))
        {
            var isInternal = symbol.DeclaredAccessibility == Accessibility.Internal;
            if (!isInternal || !symbol.IsSealed)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        HandlerVisibility,
                        declaration.Identifier.GetLocation(),
                        symbol.Name
                    )
                );
            }
        }

        if (symbol.BaseType is { } baseType && IsController(baseType))
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    ControllerForbidden,
                    declaration.Identifier.GetLocation(),
                    symbol.Name,
                    baseType.Name
                )
            );
        }

        foreach (
            var member in symbol
                .GetMembers()
                .Where(member => member.DeclaredAccessibility == Accessibility.Public)
        )
        {
            foreach (var type in ExposedTypes(member))
            {
                if (IsForbiddenExposedType(type))
                {
                    context.ReportDiagnostic(
                        Diagnostic.Create(
                            PublicPersistenceType,
                            declaration.Identifier.GetLocation(),
                            member.Name,
                            type.ToDisplayString()
                        )
                    );
                }
            }
        }
    }

    private static void AnalyzeUsing(SyntaxNodeAnalysisContext context)
    {
        var usingDirective = (UsingDirectiveSyntax)context.Node;
        var containingNamespace = usingDirective
            .Ancestors()
            .OfType<BaseNamespaceDeclarationSyntax>()
            .FirstOrDefault()
            ?.Name.ToString();
        if (
            containingNamespace is null
            || (
                !containingNamespace.Contains(".Domain", StringComparison.Ordinal)
                && !containingNamespace.Equals("Raccord.SharedKernel", StringComparison.Ordinal)
            )
        )
        {
            return;
        }

        var namespaceName = usingDirective.Name?.ToString();
        if (namespaceName is not null && IsForbiddenDomainNamespace(namespaceName))
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    DomainFrameworkReference,
                    usingDirective.Name!.GetLocation(),
                    namespaceName
                )
            );
        }
    }

    private static bool IsModuleType(INamespaceSymbol? namespaceSymbol) =>
        namespaceSymbol is not null
        && namespaceSymbol
            .ToDisplayString()
            .StartsWith("Raccord.Modules.", StringComparison.Ordinal);

    private static bool IsDomainNamespace(INamespaceSymbol? namespaceSymbol) =>
        namespaceSymbol is not null
        && (
            namespaceSymbol.ToDisplayString().Contains(".Domain", StringComparison.Ordinal)
            || namespaceSymbol
                .ToDisplayString()
                .Equals("Raccord.SharedKernel", StringComparison.Ordinal)
        );

    private static bool IsController(INamedTypeSymbol type) =>
        type.ToDisplayString()
            is "Microsoft.AspNetCore.Mvc.Controller"
                or "Microsoft.AspNetCore.Mvc.ControllerBase"
        || (type.BaseType is not null && IsController(type.BaseType));

    private static bool IsForbiddenDomainNamespace(string namespaceName) =>
        ForbiddenDomainPrefixes.Any(prefix =>
            namespaceName.StartsWith(prefix, StringComparison.Ordinal)
        );

    private static bool IsForbiddenExposedType(ITypeSymbol type)
    {
        var name = type.ContainingNamespace?.ToDisplayString() ?? string.Empty;
        return name.Contains(".Domain", StringComparison.Ordinal)
            || name.Contains(".Persistence", StringComparison.Ordinal)
            || name.Contains(".Infrastructure", StringComparison.Ordinal)
            || name.Contains("EntityFrameworkCore", StringComparison.Ordinal);
    }

    private static IEnumerable<ITypeSymbol> ExposedTypes(ISymbol member)
    {
        return member switch
        {
            IFieldSymbol field => [field.Type],
            IPropertySymbol property => [property.Type],
            IMethodSymbol method =>
            [
                method.ReturnType,
                .. method.Parameters.Select(parameter => parameter.Type),
            ],
            _ => [],
        };
    }
}
