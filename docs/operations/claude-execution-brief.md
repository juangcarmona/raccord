# Claude execution brief

Work in `C:\dev\juangcarmona\raccord`.

Raccord is a greenfield, open-source, self-hostable product for audiovisual production operations and invoicing. It is an independent system, not a layer over another operations platform. The repository already contains the initial product, architecture, and research scaffold. Read `AGENTS.md` and this document before changing code.

## Non-negotiable technology choices

- .NET 10 and C#.
- .NET Aspire for local orchestration.
- React + TypeScript for the frontend.
- C# Minimal APIs for the HTTP surface.
- Modular monolith first; no microservices without an ADR.
- Roslyn/.NET analyzers, Roslynator, and Meziantou analyzers enabled.
- Nullable reference types and warnings as errors.
- TDD on confirmed public seams, with vertical red-green-refactor slices.
- Docker Compose for self-hosting and local infrastructure.
- Azure Bicep for a documented Azure deployment path.

## Research scope

Use primary sources only for changing or legal/technical claims:

- For capability discovery only, the public documentation of existing audiovisual operations tools may be studied (categories, articles, changelogs, API and webhook references). Keep source-specific notes in the git-ignored `.private/research/` folder.
- AEAT and BOE materials for SIF/VERI*FACTU and Spanish B2B e-invoicing.

Convert the discovered capabilities into an independent Raccord capability map, domain vocabulary, user stories, acceptance criteria, and prioritized roadmap. Public research notes in `docs/research/` describe the domain in Raccord's own words; official AEAT/BOE source links are kept there. Do not build a connector, API dependency, synchronization workflow, or product identity around the source platform. Do not copy third-party wording, screens, information architecture, visual identity, or implementation. Use research to understand the problem space while building Raccord's own system of record.

## Expected deliverables

1. Complete Product Shape documentation from the actual user workflow.
2. Complete arc42 sections and ADRs for non-trivial choices.
3. Define the first vertical slice and its public seams before implementing it.
4. Scaffold the React frontend without coupling it to domain internals.
5. Implement Minimal API endpoints through application use cases.
6. Add tests first for the selected slice and keep the suite green.
7. Add architecture tests or analyzers where a rule can be made executable.
8. Add Docker Compose, Aspire resources, and Azure/Bicep templates without secrets.
9. Add CI checks for restore, format, analyzers, tests, build, and dependency vulnerabilities.
10. Report assumptions, unresolved questions, changed files, and verification results.

## First vertical slice candidate

Project -> select billable lines -> create invoice draft -> finalize invoice -> render PDF -> record audit event.

Do not implement VERI*FACTU submission yet. First separate the invoice domain, the compliance adapter, and the accounting/e-invoicing adapters so the legal implementation can be reviewed independently.

## Quality bar

- No placeholder weather endpoint or template code in the final scaffold.
- No external-platform dependency in the core product; import tooling is optional and isolated.
- No mutable finalized invoices.
- No untested money/tax calculations.
- No claim of legal compliance without verified requirements and review.
