# ADR-0015: Build and quality gates

- Status: accepted
- Date: 2026-10-06

## Decision

- **SDK pinning:** `global.json` (10.0.100, roll forward to latest feature band).
- **Central package management** (`Directory.Packages.props`) with transitive pinning; NuGet audit enabled for all packages.
- **Analyzers:** .NET analyzers (`latest-recommended`), Roslynator, Meziantou; nullable; `TreatWarningsAsErrors`; code style enforced in build. Suppressions are scoped in `.editorconfig` with a reason (for example: test naming, `ConfigureAwait` in the ASP.NET host, generated migrations).
- **Line endings:** LF everywhere (`.editorconfig` + `.gitattributes`); containers and shell scripts require it. (The initial scaffold specified CRLF.)
- **CI** (`.github/workflows/ci.yml`): restore, `dotnet format --verify-no-changes`, Release build, all tests (with Docker), vulnerable package check, pending-model-changes check; frontend lint, type check, tests, build, `npm audit --audit-level=high`, OpenAPI contract drift; container image build, Compose validation, Bicep build and lint.
- **Aspire CLI bundle:** the AppHost runs from NuGet packages (`AspireUseCliBundle=false`), so CI and contributors need no extra CLI.
- New production dependencies require an ADR (AGENTS.md).

## Production dependencies added in the first slice

Npgsql EF Core provider and Aspire client integrations (PostgreSQL, Azure Blob) — ADR-0004/0009; Aspire hosting packages for PostgreSQL, Azure Storage and JavaScript (AppHost only) — ADR-0013; frontend runtime packages — ADR-0011. The `MessagePack` pin in the AppHost was removed after upgrading Aspire to 13.6 (no vulnerable packages reported).
