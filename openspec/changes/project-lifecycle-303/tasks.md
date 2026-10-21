<!-- pdac-scope: cited -->
# Tasks

- [x] Add the fixed project status model, default Draft status, manual status mutation and domain tests.
- [x] Persist project status with an EF Core migration.
- [x] Expose project status in API responses and add the manual status endpoint.
- [x] Add project status selection in the project detail page and status in project listings.
- [x] Add a browser journey proving status changes persist after reload and register it in the PR E2E selection.
- [x] Regenerate the OpenAPI and frontend API contracts for the project edit and status endpoints and response fields.
- [x] Edit a project's name and date range through the project detail API and screen, with domain validation and a reloadable browser journey.
- [x] Allow changing the selected customer on a project and prove persistence through a browser reload.
- [x] Edit the optional external reference, reject values over 200 characters without mutation, and verify persistence after a browser reload.
- [x] Persist a selected project contact person, reject a person belonging to another customer, and verify the selection through the project detail browser flow.
- [x] Edit project type and team through the project detail screen and verify both selections persist after browser reload.
- [ ] Assign the next unique project number on create when omitted and when cleared during edit; accept custom numbers on create and edit while preserving the number for legacy update requests that omit the field.
- [ ] Add a deterministic project-number backfill ordered by project id, initialize the sequence for populated and empty databases, serialize allocations/custom assignments transactionally, skip numbers used by custom assignments, and retain a unique index as the database invariant.
- [ ] Verify custom assignment, automatic allocation, duplicate rejection without mutation, and concurrent number allocation through domain, API, persistence and browser coverage.
- [ ] Add the accepted project details workflow for existing contact and contact-person selection, project commercial overrides, account manager, notes, tasks, and files, plus duplicate project number validation; cover API validation and persisted detail display for each field.
- [x] Add domain and persistence models for planning and usage periods with invariant tests.
- [ ] Expose period designation, create/edit/duplicate/order and internal/external document-display operations through the Projects API; prove linked equipment and functions update on date changes, unlinked records remain unchanged, and failed propagation leaves the period and records consistent.
- [ ] Add project detail UI for all accepted editable fields and named periods; prove each period's internal/external visibility setting is reflected in document rendering.
- [x] Enforce Complete as a read-only operational state for project details, billable lines, equipment planning, task boards, schedules, time entries, context entries, crew functions and time periods; prove explicit reopening restores operations through API and browser journeys.
- [x] Add selection of one or more periods to generate Crew and/or Transport functions through the existing application boundary; for each requested kind, create exactly one linked function per selected period and verify eligibility, links and failure atomicity.
- [x] Add manual status effects, cancellation action discovery and explicit confirmation, including persistent warning for remaining work.
- [x] Prove status and cancellation leave issued invoices and invoiced work unchanged.
- [x] Add project edit and manual status journeys to the PR E2E selection.
- [x] Add Playwright cancellation journeys for open tasks, crew and vehicle assignments, invitations, persistent warnings and issued invoice history.
- [x] Add Playwright journeys for schedule validation, period ordering and linked/unlinked propagation, including dates outside planning and invalid usage ranges.
- [x] Add Playwright journeys for Complete blocking edits and new planning, reopening and status-dependent availability across the project areas.
- [x] Add a Playwright journey that selects multiple periods and a function kind, then verifies exactly one linked function per selected period, plus internal/external document rendering for visible and hidden periods.
- [x] Add a cancellation journey that selects only a subset of applicable actions and proves unselected actions are untouched; preserve the existing persistent-warning and invoice-history assertions.
- [x] Ensure every PR E2E selection includes the required synthetic data, `Area=projects` metadata and manifest registration.
- [x] Update affected architecture and operations documentation.
- [ ] Run the repository Done checks and reconcile all evidence.

<!-- pdac:cite id="UC-PRJ-EDIT-PROJECT-DETAILS" digest="sha256:cc87eb90988fdbd32fb2e626a02aa7154783ffc7b161702a3c2bf4ace9c2bdb7" -->
<!-- pdac:cite id="UC-PRJ-MANAGE-TIME-SCHEDULE" digest="sha256:63f5e75776fe129ece805663868a322547da608fd04023944189c071b9c04bae" -->
<!-- pdac:cite id="UC-PRJ-CHANGE-PROJECT-STATUS" digest="sha256:1a5c43dfd663a08a833301fa25ccad7dc02057bfbc8c444f4fb18b3347905acc" -->
<!-- pdac:cite id="UC-PRJ-CANCEL-PROJECT" digest="sha256:80e15e9d11dba906cb7d753fd78b7ce8b29c30651bfe9cdb9ac1d7cdc3118d96" -->
<!-- pdac:cite id="BR-PRJ-DEFAULT-PERIOD-DESIGNATION" digest="sha256:d58ad77fac1f84439e20b7fe8103c4a4918ce1e8a31485a20a0e3e93ccb1d06d" -->
<!-- pdac:cite id="BR-PRJ-PERIOD-DOCUMENT-DISPLAY" digest="sha256:aec83cc960f978d7a6e59f2105e5c15f9e75d0c855f7a5d44fa543756c54c907" -->
<!-- pdac:cite id="BR-PRJ-FUNCTIONS-FROM-PERIODS" digest="sha256:335f48cbaf338557f9f01e6f1b49e6278b02d4a9d5c73328729650d909e330ac" -->
<!-- pdac:cite id="TERM-PROJECT-STATUS" digest="sha256:ea57a30386b9806577b0ed5376630ed6b9048a5afc882cc20320958ed8382eef" -->
<!-- pdac:cite id="BR-PRJ-EXTERNAL-REFERENCE-PRINTING" digest="sha256:97cf1e3e9d16398907ff2f91d1025afaf15865c4a24c1c6e6c11ef684bd97810" -->
<!-- pdac:cite id="TERM-PROJECT-EXTERNAL-REFERENCE" digest="sha256:83edd787e70044c969325194692ecf45c1adce1686e3029ce2b8c378590ca420" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-SELECTION-EFFECTS" digest="sha256:8a97ed06931ff2b4ec14c11e4df84a1e1fb74a3599ee568ca195272d4bc541b7" -->
<!-- pdac:cite id="BR-PRJ-PARTIES-FROM-CONTACTS" digest="sha256:2fe8ce59516db4c08c3582e1c3dd6341fa3b658e8386d10f725671b633b2dcfe" -->
