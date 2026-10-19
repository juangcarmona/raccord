# Raccord agent instructions

Raccord is an open-source, self-hostable operations and invoicing platform for audiovisual production teams.

## Working agreements

- Read [docs/engineering-lifecycle.md](docs/engineering-lifecycle.md) before handling issue status, preparing a PR, invoking Fast Forward, or deciding whether integration is permitted. It defines the shared lifecycle labels and per-item review record.

- Read the relevant product and architecture documents under `docs/` before changing domain behavior.
- The product definition is Product Definition as Code (PDaC) under `docs/product/`, managed with ProductShape (`npx @prodshape/cli@0.22.0`). Product semantics change only through Product Changes in `docs/product/changes/active/`, validated with `prodshape change validate`, approved by a human and applied explicitly; never edit `docs/product/model/` directly. Reference artifact IDs instead of restating them. Every current OpenSpec document is either bound (declares `pdac-scope: cited` and carries citations emitted by `prodshape cite`) or exempt (`pdac-scope: none` with a reason a human approved); CI enforces this with `prodshape citations verify --provider openspec`.
- Keep source links and dates when updating `docs/research/`.
- Keep the core product independent from any external operations platform. Never name third-party products in public docs, UI, vocabulary or code; private research provenance goes to the git-ignored `.private/research/`.
- Implement capabilities independently with original terminology, UX, and code.
- Preserve tenant isolation, auditability, invoice numbering, immutable finalized invoices, and data exportability.
- Prefer small vertical slices with tests over speculative framework code.
- Keep deployment paths documented for Docker Compose, .NET Aspire, and Azure/Bicep.
- New production dependencies require a short decision record under `docs/adr/`.
- The required stack and executable guardrails are in `docs/architecture/architecture-rules.md`.
- Use `docs/operations/claude-execution-brief.md` as the handoff contract for large Claude implementation passes.
- Local development, tests and contract regeneration: `docs/operations/local-development.md`.

## Quality bar

- Do not claim legal or VERI\*FACTU compliance without verified implementation and review.
- Do not commit secrets, API tokens, customer data, generated PDFs, or local state.
- Follow the enforced conventions: CSharpier/Prettier formatting, Markdown linted by markdownlint-cli2 with one prose paragraph per source line (never hard-wrapped), conventional commits, `Thing_Should_Outcome` test naming, a validated OpenSpec change per meaningful change (see `docs/architecture/architecture-rules.md` and ADR-0016).
- Report assumptions, unresolved questions, and verification performed.
- At the end of each feature loop, review `docs/operations/process-ledger.md`, record durable process lessons there, and verify the resulting actions before integration.

## Red lines — stop and ask a person first

These are the counterpart to the human gates on product direction (Product Changes and OpenSpec proposals). The agent never proceeds on these alone, regardless of how the task is phrased, and asks before:

1. **Issuing, rotating or revoking any credential or key** — database passwords, storage keys, API tokens, deploy credentials, signing keys. The agent may write code that _consumes_ secrets; acting on a real secret itself is a human decision.
2. **Rewriting or force-pushing shared git history** — anything that changes commits other agents or people may already have fetched (rebase of shared branches, history filters, `--force` pushes). Rewriting your own unpushed branch is fine.
3. **Touching anything production-facing** — deployment execution (real `az`/`docker deploy` against live infrastructure), production data, production configuration, or anything under an incident. Proposing changes to deployment artifacts in the repo is normal work; executing against a live environment is not.
