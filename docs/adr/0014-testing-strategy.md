---
name: testing-strategy
description: Raccord tests only through public seams, without a mocking library, against real PostgreSQL in the backend, and drives new behaviour test-first.
status: Accepted
date: 2026-10-06
deciders: ["Juan G. Carmona"]
tags: ["testing", "tdd", "postgresql", "frontend"]
---

# ADR-0014: Test through public seams against real PostgreSQL without a mocking library

## Context

Money, tax, numbering, finalization and audit must never change without a test. Several behaviours, such as concurrency, triggers and tamper detection (ADR-0004, ADR-0005, ADR-0007), live in the database and the HTTP slice rather than in the domain alone.

## Decision

We will test at these levels, through public seams only:

| Level              | Project                                | What it proves                                                                                                              | Tools                                                                     |
| ------------------ | -------------------------------------- | --------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------- |
| Domain             | `Raccord.Domain.Tests`                 | Money, rounding, VAT, numbering, drafts, finalization, representation, parties                                              | xUnit v3                                                                  |
| Architecture       | `Raccord.Architecture.Tests`           | Dependency rules                                                                                                            | xUnit v3, reflection, csproj parsing                                      |
| Adapters           | `Raccord.Infrastructure.Tests`         | PDF structure/encoding/determinism, file storage safety                                                                     | xUnit v3                                                                  |
| HTTP + persistence | `Raccord.Web.Tests`                    | Whole slice through HTTP against real PostgreSQL: rules, concurrency, triggers, tamper detection, regeneration, SPA hosting | xUnit v3, `WebApplicationFactory`, Testcontainers (one database per test) |
| Frontend           | `src/Raccord.Frontend/**/*.test.ts(x)` | Formatting, draft and project pages against a stubbed HTTP layer                                                            | Vitest, Testing Library, jsdom                                            |

- Tests target public seams only (domain API, HTTP API, adapter ports, rendered UI). No mocking library; HTTP is stubbed at `fetch` in the frontend, and the backend uses real PostgreSQL.
- TDD: new behavior starts with a failing test at the seam. Money, tax, numbering, finalization and audit never change without a test.

## Consequences

- **Positive:**
  - Database behaviour (rules, concurrency, triggers, tamper detection) is proven against real PostgreSQL.
- **Negative:**
  - Integration tests need Docker. CI runners provide it.
- **Neutral:**
  - Tests for an ESLint-, analyzer- or format-only change are not required; behavior changes are.

## Alternatives considered

- **A mocking library and test doubles for the database:** rejected because the backend tests run against real PostgreSQL, which is where concurrency, triggers and tamper detection are enforced.

## References

- Extended by: ADR-0017
- Related: ADR-0016
