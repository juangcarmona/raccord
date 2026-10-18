<!-- pdac-scope: none reason="Implementation tasks cover E2E evidence handling and report presentation, not product behavior." -->

## 1. Establish evidence-mode guardrails

- [x] 1.1 Add a metadata-only test that enumerates the API-only journey classes across bounded contexts and fails when any lacks the explicit API evidence trait.
- [x] 1.2 Add report-builder coverage proving API-mode results display the API/test note, retain trace links, and hide screenshot/video assets.

## 2. Record and apply evidence mode

- [x] 2.1 Add the canonical browser/API evidence-mode trait and resolve it in the shared journey runner, defaulting unmarked journeys to browser mode.
- [x] 2.2 Write evidence mode in every journey result manifest; API mode skips success/failure screenshots, disables browser video recording at context creation, and retains verdict and trace.
- [x] 2.3 Update the report builder to suppress visual assets for API-mode results without changing their bounded-context grouping or result counts.

## 3. Repair API-only journey evidence

- [x] 3.1 Mark API-only journeys in Contacts, Projects, Equipment, Warehouse, Crew and transport, and Workspace with explicit API evidence mode.
- [x] 3.2 Mark API-only journeys in Invoicing, Compliance and audit, Documents and communication with explicit API evidence mode.
- [x] 3.3 Remove test-authored success pages, unrelated homepage captures, and nonstandard direct screenshot paths from API-only journeys.
- [x] 3.4 Keep mixed UI/API journeys in browser mode and capture their actual tested Raccord screen; preserve all functional assertions and synthetic data.

## 4. Verify and close the loop

- [x] 4.1 Run metadata and static report tests; verify API evidence semantics and bounded-context grouping, including multi-context journeys.
- [x] 4.2 Run the complete committed per-PR E2E selection with visual evidence; verify each result's mode, area metadata, assets and verdict in the generated report grouped by bounded context.
- [x] 4.3 Document the static public-site build prerequisite for local AppHost E2E runs; verify the build produces the input copied into the E2E test output.
- [x] 4.4 Review `docs/operations/process-ledger.md`, record any durable lesson from the escaped misleading evidence, and verify the resulting action.
