<!-- pdac-scope: none reason="Tasks add test-only AppHost coverage and verification without changing product behavior." -->

## 1. Add synthetic AppHost evidence

- [x] 1.1 Add an Invoicing API-mode AppHost journey for payments, refundable-deposit status, overdue reminder history, finance report, ledger and CSV exports; verify it passes against Aspire with synthetic data.
- [x] 1.2 Add the journey to `tests/Raccord.EndToEndTests/per-pr-selection.txt`; verify the manifest checker resolves every selected journey and metadata assigns BC-INVOICING/API evidence.

## 2. Verify and record

- [x] 2.1 Run the complete committed per-PR AppHost battery with evidence capture; verify every selected journey passes and its result is assigned to Invoicing and Commercial.
- [x] 2.2 Run OpenSpec validation, ProductShape citation verification, formatter/analyzer checks and the relevant deterministic CI lane; record the outcomes and review `docs/operations/process-ledger.md` for durable lessons.
