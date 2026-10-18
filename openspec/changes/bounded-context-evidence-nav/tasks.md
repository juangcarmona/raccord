<!-- pdac-scope: none reason="Implementation tasks cover test metadata and engineering evidence, not product behavior." -->

## 1. Classify E2E journeys

- [x] 1.1 Add canonical Area values for the ten product bounded contexts and Cross-cutting, then assign every concrete E2E class to one or more groups; a focused reflection test verifies every concrete browser journey declares only canonical areas (1/1 passed).
- [x] 1.2 Persist the concrete class's distinct Area values in each journey result manifest; verified all 33 fresh run manifests contain context assignments and passed statuses.
- [x] 1.3 Keep every selected journey in the committed per-PR manifest; `.github/scripts/check-e2e-selection.sh` confirms the committed manifest resolves to all 33 discovered journeys.
- [x] 1.4 Add invoicing AppHost journeys for a blank and explicit issue date; verify `null` delegates to the organization date and an explicit date survives finalization.
- [x] 1.5 Verify the Contacts schema migration preserves an existing customer row with a PostgreSQL migration test; 1/1 passed. This criterion is database migration coverage, not a browser journey.
- [x] 1.6 Add API-only Workspace evidence for updating a custom role's permissions; the full AppHost battery passes it, while user assignment remains a recorded gap because no user-facing workflow exists.
- [x] 1.7 Keep metadata guard checks outside the reportable journey manifest and run them as a separate CI check; verified the metadata-only filter discovers and passes exactly one check.

## 2. Group the evidence report

- [x] 2.1 Group left-navigation links under the ten bounded-context headings, Cross-cutting, and Unclassified; verify each heading and its linked journeys in report-builder tests.
- [x] 2.2 Render empty contexts with an explicit no-evidence state and keep multi-context journeys linked in each applicable group without duplicating report assets; verify empty and multi-context report cases.
- [x] 2.3 Preserve historical manifests without area metadata under Unclassified; verify an old-format manifest still renders its journeys and assets.
- [x] 2.4 Preserve accessible navigation behavior at desktop and narrow viewports; verify the generated report's navigation labels, links, and responsive controls.
- [x] 2.5 Attach mobile and publication screenshots to their owning journey; verify they appear with that journey and do not add unknown report rows.
- [x] 2.6 Attach explicitly declared supplemental screenshots to their result manifest owner; verify design-system captures stay under their Workspace or Cross-cutting journey without creating unknown rows.

## 3. Run and capture evidence

- [x] 3.1 Run static evidence report tests and the Contacts E2E journey with visual capture enabled; verify the Contacts navigation group links to its screenshot and result.
- [x] 3.2 Run the complete committed per-PR E2E selection; verify all selected journeys pass and retain the correct context labels in generated results; 33/33 passed with zero unknown context assignments.
- [x] 3.3 Build the engineering evidence site and inspect the generated left navigation for all ten contexts, empty groups, and cross-cutting journeys.
- [x] 3.4 Run a database-backed E2E journey against the session PostgreSQL resource; verify startup succeeds with a clean database while the developer's named PostgreSQL volume remains untouched.
- [x] 3.5 Run the expanded per-PR selection and capture refreshed bounded-context evidence for the new Projects and Invoicing journeys; 33/33 passed, with all ten context groups represented in the report.
- [x] 3.6 Keep invoice journeys order-independent by using the organization-local current date for default and explicit-date issuance; rebuild and rerun the complete selection with refreshed context-grouped evidence; 33/33 passed.
- [x] 3.7 Run the expanded selection with the custom-role update journey; verified 34/34 journeys passed, the Workspace result is classified, and its API trace is retained in the grouped report.
- [x] 3.8 Review the process ledger, record the metadata-test selection lesson, and verify the separate metadata check (1/1), committed manifest (34 journeys), and refreshed report (34 passed, 0 unknown).
