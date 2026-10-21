<!-- pdac-scope: cited -->

## Why

Issue #418 needs durable VAT class and scheme catalogs so Contacts and Projects can store stable tax configuration instead of free text. The approved and applied Product Change `CHG-FINANCE-CATALOGS-417-419-001` defines a source-dated 21% Spanish standard-rate setup default, administrator-configured additional classes and schemes, and immutable history for rates already used.

## What Changes

- Add the Invoicing-owned class and scheme catalogs, with stable IDs, workspace isolation, authorized administration, and validation.
- Initialize a new installation's standard class and scheme from one named, source-dated project constant for 21%; persist that value as ordinary configuration and never reapply the constant over saved configuration.
- Let administrators configure additional per-class, fixed-rate, and reverse-charge schemes without prefilled reduced, zero-rate, or reverse-charge percentages.
- Expose configured schemes through module contracts and API by stable identity for consumption by Contacts and Projects; #300 owns contact-default fields and their editing UI.
- Reject edits that would alter a rate already used by a saved commercial record; represent later rate changes with a new scheme.
- Preserve stable scheme identity and percentage snapshots on saved project billable lines and invoice draft lines; serialize first use against concurrent rate changes.
- Append catalog setup, creation, update, and default-change audit events in the same transaction as each catalog mutation.
- Add domain, persistence, API, authorization, frontend, contract, and selected browser E2E coverage.

## Capabilities

### New Capabilities

- `vat-catalogs`: administrators configure VAT classes and schemes; new Spanish installations receive a dated standard-rate default; Contacts and Projects can consume configured schemes by stable identity; used rates remain historically stable.

### Modified Capabilities

- Existing audit trail: VAT catalog changes use the append-only audit behavior and same-operation transaction contract.

## Impact

Issue #418, Invoicing module contracts and implementation, database schema and migration, root API composition, generated OpenAPI and frontend client, financial settings UI, stable catalog queries for Contacts and Projects, authorization, and the per-PR Playwright selection. Contact-default fields and their editing UI remain in #300. This implements `CHG-FINANCE-CATALOGS-417-419-001` from PR #420 and depends on the applied product baseline in that stacked PR. No new production dependency is expected.

<!-- pdac:cite id="UC-FIN-CONFIGURE-VAT" digest="sha256:3c05cb10f788f77cb6752fca0c806d089d618a79be13a6cd2a2d4cb911388ab7" -->
<!-- pdac:cite id="FR-FIN-VAT" digest="sha256:bde0ed771d8db4d834acb66d2b278fef674664670ac720dd9c91dbde99e1c7df" -->
<!-- pdac:cite id="TERM-VAT-CLASS" digest="sha256:3ad27a6e017bcec71c1452669ff354b4c0fc8f29725635de0a90c01421d287c2" -->
<!-- pdac:cite id="TERM-VAT-SCHEME" digest="sha256:d7fbf3cf62480bae491180a199e8fbe4455b25712d349af2b203173abf42095f" -->
<!-- pdac:cite id="TERM-VAT-SCHEME-KIND" digest="sha256:b7a466e19572ce3eae02fc09efab7e1adbc34b940bf889d2d32984ee79d508cf" -->
<!-- pdac:cite id="BR-FIN-VAT-FROM-CLASS-AND-SCHEME" digest="sha256:7ec9486b4513c0492fa7b7b53718f358db9717f8b620d1ee9d1f839e791c6f2a" -->
<!-- pdac:cite id="BR-FIN-USED-RATE-FROZEN" digest="sha256:b33134bd0f2519dfb830cd37b214eab8e6c9e8609b7a9bf63c9db8e2e6cb8a95" -->
<!-- pdac:cite id="BR-FIN-DEFAULT-VAT-CLASS" digest="sha256:7f539a8bdefbc8cea1877191ba869addd68c206993c40a5f791f8bf96a09049b" -->
<!-- pdac:cite id="FR-AUDIT-001" digest="sha256:7646b5398f69286e802b2a3175edcf8b18cfc3b742edc9d3c2a44476c6159417" -->
