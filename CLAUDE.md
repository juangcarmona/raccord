# Raccord

Read `AGENTS.md` first. It is the shared source of truth for repository conventions.

The required stack is .NET Aspire, React/TypeScript, C# Minimal APIs, modular-monolith architecture, Roslyn analyzers, and TDD. Read `docs/architecture/architecture-rules.md` and `docs/operations/claude-execution-brief.md` before implementation.

Enforced conventions (see ADR-0016 and `docs/operations/local-development.md`): CSharpier/Prettier formatting, conventional commits via husky hooks, `Thing_Should_Outcome` test naming with Shouldly, a validated OpenSpec change per meaningful change, and AGENTS.md's red lines (credentials, shared history, production) — stop and ask a person first.

Before implementation, inspect:

1. `docs/product/` (PDaC model and Product Changes)
2. `docs/architecture/arc42/`
3. `docs/research/`
4. `docs/architecture/decisions/`

Use the smallest vertical slice that proves a real audiovisual production workflow. Keep all external imports outside the core domain and implement original terminology, UX, and code.

For every meaningful change, update the relevant documentation and add tests. Report assumptions, unresolved questions, and verification performed.
