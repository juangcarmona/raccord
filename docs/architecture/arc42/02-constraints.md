# 2. Constraints

## Technical

- .NET 10 / C#, ASP.NET Core Minimal APIs, .NET Aspire 13 for local orchestration.
- React + TypeScript frontend; it may only use the public HTTP contract.
- PostgreSQL as the system of record; object storage for binary documents.
- Modular monolith (ADR-0001); dependency direction enforced by tests (ADR-0003).
- Nullable reference types, .NET analyzers, Roslynator, Meziantou, warnings as errors.
- TDD with vertical red-green-refactor slices.
- Docker Compose for self-hosting; Azure deployment described in Bicep without secrets.

## Organizational and legal

- AGPLv3 code; the Raccord brand is not licensed by the code license (ADR-0002).
- Raccord must run without any external operations platform. Optional import tooling reads customer-provided files and lives outside the core domain.
- No third-party wording, screens, branding or implementation is reused; capability research stays private (see [research](../../research/README.md)).
- Spanish tax requirements come from official AEAT/BOE sources; legal compliance is not claimed without review.
- No secrets, customer data, generated PDFs or local state in Git.

## Conventions

- English for code and technical documentation; Spanish and English UI.
- LF line endings; central package management; ADR for every new production dependency.
