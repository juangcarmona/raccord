<!-- pdac-scope: cited -->

# Design

## Context

See `proposal.md` for why this reconciliation is needed. On current `main`, ADR-0023 already records the frontend feature-slice architecture and ADR-0028 is the newest ADR. The self-hosted responsibility boundary is stated in `CON-SIF-REGULATION`; the compliance constraint also prohibits claims until verification. Both ProductShape artifacts currently report `draft` status. The old PR #99 created an ADR-0023 on a divergent feature stack and does not contain separate written legal advice.

<!-- pdac:cite id="CON-SIF-REGULATION" digest="sha256:a3922433f348bd87649eede3e8a57ca51fba868ad49927883feac172775f8dc3" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->

## Goals / Non-Goals

**Goals:**

- Put the existing deployment boundary and its rationale in the current architecture record under the next unused number, ADR-0029.
- Align the ADR index, arc42 decision index, risk register and compliance-seam overview with that record.
- Preserve the independent legal-review and verified-implementation gates; describe no statutory conclusion beyond the product boundary already recorded in ProductShape.

**Non-Goals:**

- Change ProductShape, invoice behavior, compliance submission, certificates, tax semantics or any application/API contract.
- Treat the prior stacked merge as an integration into `main` or reuse its conflicting ADR-0023 number.

## Decisions

- **Add ADR-0029 rather than reusing ADR-0023.** Current `main` already uses ADR-0023 for frontend architecture; ADR-0028 is the highest existing record. Copying the old number would create a collision. The ADR will describe the product/deployment responsibility boundary, cite the current ProductShape constraints and research open question, and explicitly state that it does not establish statutory liability or prove independent legal advice.
- **Keep ProductShape unchanged.** The current model already contains the boundary, and this work reconciles its architectural explanation. Any change to product meaning or draft status requires the Product Change workflow.
- **Change only the documents named in the proposal.** The indexes point to ADR-0029; arc42 risk R-2 and the compliance-seam overview summarize and link to it. The detailed rationale stays in one ADR.
- **Keep fiscal implementation gates separate.** The impact graph connects the cited constraints to `BC-COMPLIANCE`, `BC-INVOICING`, `FR-COMPLIANCE-001`, `FR-COMPLIANCE-002` and `FR-INVOICE-006`. These neighbors are not modified; their product approvals, official specifications and legal review remain independent prerequisites.
- **Record the branch-integration lesson in the process ledger.** A PR merged into a feature branch is not present in `main`; verify default-branch ancestry and actual files before closing work.

## Risks / Trade-offs

- [Risk] The ADR could be read as legal advice → State that it records Raccord's product/deployment boundary, cite the current model, and preserve the independent legal-review gate.
- [Risk] The old ADR number could collide with current main → Use ADR-0029, confirmed against the current ADR index.
- [Risk] An issue could again be treated as done after a stacked merge → Record and verify a default-branch ancestry/file-presence check.

## Migration Plan

No runtime or data migration. Merge the reviewed documentation PR to `main`; verify ADR-0029 and its index references on the resulting default-branch commit. Until that merge, the issue remains open.

## Open Questions

None for this documentation reconciliation. The absence of a separately linked legal opinion is recorded as a limitation; it does not change this scope or permit a compliance claim.
