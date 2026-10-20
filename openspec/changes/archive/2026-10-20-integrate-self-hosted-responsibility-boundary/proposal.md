<!-- pdac-scope: cited -->

## Why

PR #99 recorded the self-hosted responsibility boundary but merged into `feat/20-templates`; its decision is absent from `main`. The current ProductShape model already states the operator boundary, so this change reconciles the architecture records with that baseline while preserving the legal-review and no-compliance-claim gates.

## What Changes

- Add ADR-0029 for the self-hosted installation responsibility boundary, adapting PR #99 to the current ADR sequence and making no claim that an external legal opinion was supplied.
- Update the ADR and arc42 decision indexes, risk R-2, and the compliance-seam overview to reference ADR-0029.
- Add a process-ledger lesson to verify stacked PR ancestry against the default branch before treating an issue as integrated.
- Leave ProductShape and runtime behavior unchanged; keep independent legal review and official specification checks as separate prerequisites.

## Capabilities

### New Capabilities

None. This is a documentation-only reconciliation.

### Modified Capabilities

None. No product or implementation requirement changes. `skip_specs: true` is set because this change introduces no behavior.

## Product definition impact

The change relies on the current responsibility boundary and the no-compliance-claim rule. They remain draft ProductShape artifacts and are cited as current model content, not as proof of external legal advice or regulatory compliance. The direct impact neighbors `BC-COMPLIANCE`, `BC-INVOICING`, `FR-COMPLIANCE-001`, `FR-COMPLIANCE-002`, and `FR-INVOICE-006` were inspected and are not changed.

<!-- pdac:cite id="CON-SIF-REGULATION" digest="sha256:a3922433f348bd87649eede3e8a57ca51fba868ad49927883feac172775f8dc3" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->

## Impact

- `docs/adr/0029-self-hosted-verifactu-responsibility.md`
- `docs/adr/README.md`
- `docs/architecture/arc42/09-architecture-decisions.md`
- `docs/architecture/arc42/11-risks-and-technical-debt.md`
- `docs/architecture/compliance-seam.md`
- `docs/operations/process-ledger.md`
- No application code, API, database, deployment, credentials, or E2E journeys change.
