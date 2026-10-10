# ADR-0014: Testing strategy

- Status: accepted
- Date: 2026-10-06

## Decision

| Level | Project | What it proves | Tools |
|---|---|---|---|
| Domain | `Raccord.Domain.Tests` | Money, rounding, VAT, numbering, drafts, finalization, representation, parties | xUnit v3 |
| Architecture | `Raccord.Architecture.Tests` | Dependency rules | xUnit v3, reflection, csproj parsing |
| Adapters | `Raccord.Infrastructure.Tests` | PDF structure/encoding/determinism, file storage safety | xUnit v3 |
| HTTP + persistence | `Raccord.Web.Tests` | Whole slice through HTTP against real PostgreSQL: rules, concurrency, triggers, tamper detection, regeneration, SPA hosting | xUnit v3, `WebApplicationFactory`, Testcontainers (one database per test) |
| Frontend | `src/Raccord.Frontend/**/*.test.ts(x)` | Formatting, draft and project pages against a stubbed HTTP layer | Vitest, Testing Library, jsdom |

- Tests target public seams only (domain API, HTTP API, adapter ports, rendered UI). No mocking library; HTTP is stubbed at `fetch` in the frontend, and the backend uses real PostgreSQL.
- TDD: new behavior starts with a failing test at the seam. Money, tax, numbering, finalization and audit never change without a test.

## Consequences

- Integration tests need Docker. CI runners provide it.
- Tests for an ESLint-, analyzer- or format-only change are not required; behavior changes are.
