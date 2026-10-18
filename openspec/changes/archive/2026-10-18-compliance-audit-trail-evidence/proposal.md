<!-- pdac-scope: none reason="This change adds test-only browser evidence for existing invoice audit behavior and does not change product behavior or scope." -->

## Why

The selected E2E suite exercises invoice export, inbound e-invoice status and workday audit data, but it does not open an issued invoice and verify the user-visible audit trail. Add one synthetic Aspire browser journey to prove the supported invoice audit workflow in its owning bounded contexts.

## What Changes

- Add a selected browser journey that creates and issues a synthetic invoice, then verifies its audit trail shows the recorded actions, actor and timestamp.
- Group the journey under Compliance and audit and Invoicing in the per-PR evidence report.
- Keep database invariants, legal questions, tax compliance and external submission outside the browser proof.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

None. This is test-only evidence for already implemented behavior; `.openspec.yaml` opts out of spec deltas.

## Impact

Only `tests/Raccord.EndToEndTests/` and this OpenSpec change are affected. No product behavior, API contract, database, dependency, deployment configuration, credential, legal interpretation or compliance claim changes.

## Definition of Done

- Acceptance: the selected Aspire browser journey shows the issued and archived actions, their actor and chronological timestamps under both bounded contexts.
- Implementation and tests: the task list matches the diff; metadata and selection checks, the focused journey and the full per-PR Aspire battery pass.
- Quality and architecture: CSharpier, Release build/analyzers, backend tests, Markdown formatting/lint, OpenSpec validation and citation verification pass; no runtime dependency or architecture boundary changes.
- Security and product: no credentials or external endpoints are used; no ProductShape change or product behavior is introduced; compliance claims remain explicitly out of scope.
- Evidence and review: the exact PR head has green CI, the successful journey artifact is grouped under Compliance and Invoicing, and the Owner reviews the diff.
- Limitations: tax-law advice, chained billing records, per-rate VAT regime/exemption fields, authority QR rendering and submission remain outside this change and on their existing follow-up items.
