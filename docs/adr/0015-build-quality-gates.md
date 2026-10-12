---
name: build-quality-gates
description: Raccord pins its SDK and packages, treats analyzer warnings as errors, and runs formatting, tests, vulnerability, migration and contract-drift checks in CI.
status: Accepted
date: 2026-10-06
deciders: ["Juan G. Carmona"]
tags: ["build", "ci", "analyzers", "dependencies", "quality"]
---

# ADR-0015: Enforce build and quality gates in the build and in CI

## Context

Contributors and CI must build the same way: one SDK, known package versions, analyzers and code style, and line endings that containers and shell scripts accept. Generated artifacts such as migrations and the OpenAPI contract must stay in step with the code, and dependencies must be checked for known vulnerabilities.

## Decision

We will enforce these gates in the build and in CI:

- **SDK pinning:** `global.json` (10.0.100, roll forward to latest feature band).
- **Central package management** (`Directory.Packages.props`) with transitive pinning; NuGet audit enabled for all packages.
- **Analyzers:** .NET analyzers (`latest-recommended`), Roslynator, Meziantou; nullable; `TreatWarningsAsErrors`; code style enforced in build. Suppressions are scoped in `.editorconfig` with a reason (for example: test naming, `ConfigureAwait` in the ASP.NET host, generated migrations).
- **Line endings:** LF everywhere (`.editorconfig` + `.gitattributes`); containers and shell scripts require it. (The initial scaffold specified CRLF.)
- **CI** (`.github/workflows/ci.yml`): restore, `dotnet format --verify-no-changes`, Release build, all tests (with Docker), vulnerable package check, pending-model-changes check; frontend lint, type check, tests, build, `npm audit --audit-level=high`, OpenAPI contract drift; container image build, Compose validation, Bicep build and lint.
- **Aspire CLI bundle:** the AppHost runs from NuGet packages (`AspireUseCliBundle=false`), so CI and contributors need no extra CLI.
- New production dependencies require an ADR (AGENTS.md).

### Production dependencies added in the first slice

Npgsql EF Core provider and Aspire client integrations (PostgreSQL, Azure Blob) — ADR-0004/0009; Aspire hosting packages for PostgreSQL, Azure Storage and JavaScript (AppHost only) — ADR-0013; frontend runtime packages — ADR-0011. The `MessagePack` pin in the AppHost was removed after upgrading Aspire to 13.6 (no vulnerable packages reported).

## Consequences

- **Positive:**
    - Contributors and CI build with the same SDK and package versions, and style, analyzer, vulnerability, migration and contract-drift problems fail the build.
- **Negative:**
    - Every warning fails the build; a suppression needs a scoped `.editorconfig` entry with a reason.
- **Neutral:**
    - New production dependencies require an ADR.

## Alternatives considered

- **CRLF line endings, as the initial scaffold specified:** rejected because containers and shell scripts require LF.
- **The Aspire CLI bundle:** rejected (`AspireUseCliBundle=false`) so CI and contributors need no extra CLI.

## References

- Extended by: ADR-0016
- Related: ADR-0004, ADR-0009, ADR-0011, ADR-0013
