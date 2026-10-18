# Raccord architecture rules

Guardrails for contributors and coding agents. Rules marked **(enforced)** fail the build or CI.

## Application shape

- .NET Aspire for local orchestration and service discovery.
- C# Minimal APIs for HTTP endpoints; the root host exposes module endpoints and maps errors at the module boundary (`src/root/Raccord.Web`).
- React for the user interface; it consumes only the generated OpenAPI contract **(enforced: contract drift check)**.
- The frontend composes `AuthProvider` and `RequireAuth` in `src/main.tsx` and `src/app/routes.tsx`; feature routes must not bypass the authenticated application boundary.
- Modular monolith. No distributed services without an ADR and a measured reason.
- Feature/domain modules with explicit, small public interfaces. Endpoints never reach persistence directly.

## Dependency rules

```text
Root host -> module implementations -> module contracts -> SharedKernel
Root host -> BuildingBlocks + ServiceDefaults
Module implementations -> their contracts + SharedKernel + BuildingBlocks
BuildingBlocks -> SharedKernel
AppHost -> deployable projects only
Module contracts -> SharedKernel and other module contracts only
```

- Project references follow this graph **(enforced: `DependencyRulesTests`)**.
- SharedKernel and module contracts reference only approved base libraries; module implementations do not reference the root host or unrelated persistence adapters **(enforced)**.
- Adapters sit at ports defined by module contracts; external vocabularies (database, AEAT XSD, e-invoice syntaxes) never enter the domain.
- Cross-module writes go through application use cases in one unit of work.
- Persistence models do not become API contracts; contracts live beside their module.

## Financial rules

- Money, taxes, numbering, rounding, finalization and audit change only with tests.
- Issued invoices, audit events and compliance records are append-only **(enforced by database triggers and tests)**.
- Calculations happen in the domain only; the frontend formats server values.

## Quality rules

- TDD: failing test at a public seam, minimal implementation, refactor when green.
- Nullable, .NET analyzers, Roslynator, Meziantou; warnings are errors; suppressions are scoped and justified in `.editorconfig` **(enforced)**.
- One formatter per language **(enforced)**: CSharpier for C#, ESLint (strict type-checked + jsx-a11y strict) + Prettier for the frontend, TypeScript strict mode.
- Conventional commits with a restricted type-enum **(enforced at commit time by husky/commitlint, mirrored in CI)**.
- Test names follow `Thing_Should_Outcome` and assertions use Shouldly **(enforced over every test assembly)**.
- Playwright E2E journeys over the booted Aspire AppHost; the per-PR selection is pinned by the committed manifest — a narrowed filter is a red build **(enforced)**.
- Every meaningful change ships a validated OpenSpec change (proposal, tasks, capability deltas); PDaC stays the product-model source of truth **(enforced in CI)**.
- Security gates on every PR: TruffleHog (secrets in history since base), Semgrep (in-repo rules + OWASP/secrets registry sets), Trivy fs scan **(blocking, see ADR-0016)**.
- SBOM + license policy on the built image and the runtime dependency graph — only allowlisted licenses ship in a deliverable (ADR-0002/0016) **(enforced)**.
- Stryker mutation score over the Domain layer, nightly with thresholds **(nightly, not per-PR — ADR-0016)**.
- ZAP baseline scan of the Compose stack after merge to main **(enforced, blocking on FAIL)**.
- No secrets, customer data, generated PDFs or local state in Git.
- New production dependencies require an ADR.
- Frontend imports follow `app → features → shared`; app may compose features, features may use shared, and shared/peer features may not import upward or sideways **(enforced: `npm run architecture:check` and its fixture suite)**.
- Shared visual tokens live in `docs/design/tokens.css`; app, public-site, and engineering-site styles consume that source. The dependency-free drift check runs from Husky pre-commit and an unconditional CI job **(enforced: `node scripts/check-design-system.mjs`)**.
- Agent red lines (AGENTS.md): credentials, shared history, production — stop and ask a person first.
