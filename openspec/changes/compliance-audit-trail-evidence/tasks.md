<!-- pdac-scope: none reason="Tasks add test-only browser evidence and verification without changing product behavior." -->

## 1. Add the user-visible audit journey

- [x] 1.1 Add a synthetic Aspire browser journey that issues an invoice and verifies the visible audit actions, actor and timestamps.
- [x] 1.2 Add it to the committed per-PR selection and tag its evidence under Compliance and audit and Invoicing.

## 2. Verify and close the loop

- [x] 2.1 Run the new journey against the Aspire AppHost and inspect its bounded-context evidence result.
- [x] 2.2 Run the complete committed per-PR AppHost battery with evidence capture and verify all selected journeys pass.
- [x] 2.3 Run OpenSpec validation, ProductShape citation verification, formatting and CI-equivalent deterministic checks.
- [x] 2.4 Review `docs/operations/process-ledger.md`, record any durable lesson and verify the resulting action.
