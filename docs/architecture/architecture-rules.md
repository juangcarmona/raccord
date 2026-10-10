# Raccord architecture rules

Guardrails for contributors and coding agents. Rules marked **(enforced)** fail the build or CI.

## Application shape

- .NET Aspire for local orchestration and service discovery.
- C# Minimal APIs for HTTP endpoints; endpoints only translate HTTP to application use cases (`src/Raccord.Web/Api/Endpoints.cs`).
- React for the user interface; it consumes only the generated OpenAPI contract **(enforced: contract drift check)**.
- Modular monolith. No distributed services without an ADR and a measured reason.
- Feature/domain modules with explicit, small public interfaces. Endpoints never reach persistence directly.

## Dependency rules

```text
Web/API -> Application -> Domain
Web/API -> Contracts
Application -> Contracts
Infrastructure -> Application + Domain + Contracts
AppHost -> deployable projects only
Domain, Contracts -> base class library only
```

- Project references follow this graph **(enforced: `DependencyRulesTests`)**.
- Domain and Contracts reference only the BCL; Application references no ASP.NET, EF Core, Npgsql, Azure, Aspire or HTTP assemblies; Domain, Contracts and Application add no packages **(enforced)**.
- Adapters sit at ports defined in Application; external vocabularies (database, AEAT XSD, e-invoice syntaxes) never enter the domain.
- Cross-module writes go through application use cases in one unit of work.
- Persistence models do not become API contracts; contracts live in `Raccord.Contracts`.

## Financial rules

- Money, taxes, numbering, rounding, finalization and audit change only with tests.
- Issued invoices, audit events and compliance records are append-only **(enforced by database triggers and tests)**.
- Calculations happen in the domain only; the frontend formats server values.

## Quality rules

- TDD: failing test at a public seam, minimal implementation, refactor when green.
- Nullable, .NET analyzers, Roslynator, Meziantou; warnings are errors; suppressions are scoped and justified in `.editorconfig` **(enforced)**.
- `dotnet format`, ESLint (strict type-checked + jsx-a11y strict) and TypeScript strict mode **(enforced in CI)**.
- No secrets, customer data, generated PDFs or local state in Git.
- New production dependencies require an ADR.
