# Raccord agent instructions

Raccord is an open-source, self-hostable operations and invoicing platform for audiovisual production teams.

## Working agreements

- Read the relevant product and architecture documents under `docs/` before changing domain behavior.
- The product definition is Product Definition as Code (PDaC) under `docs/product/`, managed with ProductShape (`npx @prodshape/cli@0.20.0`). Product semantics change only through Product Changes in `docs/product/changes/active/`, validated with `prodshape change validate`, approved by a human and applied explicitly; never edit `docs/product/model/` directly. Reference artifact IDs (and `prodshape cite` once accepted) instead of restating them.
- Keep source links and dates when updating `docs/research/`.
- Keep the core product independent from any external operations platform. Never name third-party products in public docs, UI, vocabulary or code; private research provenance goes to the git-ignored `.private/research/`.
- Implement capabilities independently with original terminology, UX, and code.
- Preserve tenant isolation, auditability, invoice numbering, immutable finalized invoices, and data exportability.
- Prefer small vertical slices with tests over speculative framework code.
- Keep deployment paths documented for Docker Compose, .NET Aspire, and Azure/Bicep.
- New production dependencies require a short decision record under `docs/architecture/decisions/`.
- The required stack and executable guardrails are in `docs/architecture/architecture-rules.md`.
- Use `docs/operations/claude-execution-brief.md` as the handoff contract for large Claude implementation passes.
- Local development, tests and contract regeneration: `docs/operations/local-development.md`.

## Quality bar

- Do not claim legal or VERI*FACTU compliance without verified implementation and review.
- Do not commit secrets, API tokens, customer data, generated PDFs, or local state.
- Report assumptions, unresolved questions, and verification performed.
