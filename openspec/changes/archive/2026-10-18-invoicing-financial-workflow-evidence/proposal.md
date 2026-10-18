<!-- pdac-scope: none reason="This change adds test-only AppHost evidence for already implemented invoicing routes and does not change product behavior or scope." -->

## Why

The Invoicing audit currently cites web-host integration tests for payments, refundable deposits, reminders and finance reports as if they were part of the committed AppHost E2E battery. Add selected synthetic AppHost journeys so the grouped report proves those supported workflows against the real Aspire application.

## What Changes

- Add Invoicing AppHost coverage for payment and refundable-deposit state transitions, overdue reminders, finance reporting, ledger and CSV exports.
- Keep the journeys selected in the committed per-PR E2E manifest and label their evidence under Invoicing.
- Leave invoice behavior, API contracts, legal interpretation and product scope unchanged.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

None. These are test-only changes; `.openspec.yaml` opts out of spec deltas.

## Impact

Only `tests/Raccord.EndToEndTests/` and the OpenSpec change artifacts are affected. No production code, API schema, database, dependencies, deployment configuration or public behavior changes.
