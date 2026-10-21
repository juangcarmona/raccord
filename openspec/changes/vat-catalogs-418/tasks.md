<!-- pdac-scope: cited -->

## 1. Domain contract and source-dated initialization

- [x] 1.1 Add failing domain tests for class and scheme validation, supported kinds (per-class, fixed rate, reverse charge), stable identity, and invalid percentage/reference rejection.
- [x] 1.2 Add failing tests proving the named Spanish standard-rate constant carries 21%, official source URL and verification date, and that initialization uses it only when the installation has no VAT catalog.
- [x] 1.3 Add failing domain tests proving a used scheme rate cannot change and a replacement scheme preserves existing references.
- [x] 1.4 Add failing tests proving a saved project billable line records both the stable scheme identity and effective percentage snapshot, and invoice draft lines carry both forward.
- [x] 1.5 Implement the minimal Invoicing domain types and validation to make the tests pass; keep them independent of EF Core and HTTP.
- [x] 1.6 Update the project billable-line and invoice draft-line write paths to capture the stable VAT scheme identity and effective percentage snapshot; keep existing calculation behavior intact.
- [x] 1.7 Implement the source-dated bootstrap value and first-install initializer; test restart/idempotency and preservation of administrator changes.

## 2. Persistence and module boundary

- [x] 2.1 Add PostgreSQL integration tests for create/list/update class and scheme operations, the single-workspace-per-installation boundary, default initialization, and used-rate protection.
- [x] 2.2 Add integration tests for concurrent first-use registration versus rate update, and audit-event atomicity on successful and rolled-back catalog operations.
- [x] 2.3 Add the persistence mapping, migration and repository adapter behind the Invoicing-owned contract; serialize first-use registration and rate updates in one transaction, and append audit facts in that unit of work.
- [x] 2.4 Verify rollback on invalid references, concurrent updates, existing invoice calculations and project records that already reference a scheme.

## 3. Application use cases and API

- [x] 3.1 Add application and web tests for list, create, update, set-default and invalid-operation outcomes at public API seams.
- [x] 3.2 Implement Invoicing use cases and stable catalog contracts; expose class and scheme identities and configured values for Contacts and Projects.
- [x] 3.3 Enforce the product administrator permissions on every mutating endpoint and cover unauthorized, forbidden, invalid and successful requests.
- [x] 3.4 Regenerate OpenAPI and the TypeScript client; pass the contract-drift check.

## 4. Financial settings interface

- [x] 4.1 Add frontend tests for loading classes/schemes, creating and editing an unused rate, rejecting an invalid mapping, selecting a default and displaying the source-date for the initial 21% value.
- [x] 4.2 Implement the Invoicing settings route with accessible forms, server-sourced values, localization and the generated API client; do not calculate rates in the frontend.
- [x] 4.3 Verify the UI cannot edit a used rate and shows the server's validation reason without losing unsaved input.

## 5. Browser journey and evidence

- [x] 5.1 Add an Invoicing Playwright journey that starts from a fresh installation, verifies the 21% source-dated default, configures a synthetic class and scheme, makes it default, creates and saves a project billable line using that scheme, and verifies the stable scheme identity and percentage snapshot are retained while an attempted used-rate edit is rejected.
- [x] 5.2 Cover a successful catalog mutation's audit event and a failed mutation's absence of an audit event through integration tests; assert the events are appended within the catalog transaction.
- [x] 5.3 Tag the journey `Invoicing` and add it to `tests/Raccord.EndToEndTests/per-pr-selection.txt`; verify the pinned selection includes it.
- [x] 5.4 Capture Aspire AppHost browser evidence and map every #418 acceptance criterion to a passing automated test or runtime artifact. The contact-default picker remains in #300, which consumes the scheme identity contract.

## 6. Documentation and final verification

- [x] 6.1 Update affected arc42 module and persistence descriptions plus operations guidance for the dated bootstrap default and explicit rate-change process; do not claim tax compliance.
- [x] 6.2 Review `docs/operations/process-ledger.md` and record any durable lessons from this loop.
- [ ] 6.3 Run ProductShape validation, OpenSpec validation, PDaC citation verification, formatting, analyzers, architecture tests, .NET tests, frontend checks, contract-drift checks, security/license/SBOM checks, deployment-artifact checks, E2E selection validation and the complete selected Playwright battery.
- [ ] 6.4 Obtain the Owner's Done judgement and PR approval; keep the PR stacked on #420 until that base is integrated, then verify the final head and complete the tracker transition.

## Definition of Done coverage

1. Map each acceptance criterion to automated tests or Aspire runtime evidence; the Owner confirms the mapping.
2. Complete or explicitly disposition every task above.
3. Run all CI suites with TDD, tax and historical-rate coverage.
4. Pass formatters, analyzers, EF model and API contract-drift checks.
5. Pass security, dependency, image and license gates; the Owner reviews the authorization surface.
6. Pass architecture checks and have the Owner confirm the approved design and ADR conformance.
7. Update the relevant architecture and operations docs in this PR.
8. Pass ProductShape, OpenSpec and citation validation; the Owner confirms product/spec/code consistency.
9. Link green head-commit CI and attach UI runtime evidence.
10. Obtain Owner PR approval before integration.
11. State limitations and follow-up issues in the PR description.

<!-- pdac:cite id="UC-FIN-CONFIGURE-VAT" digest="sha256:3c05cb10f788f77cb6752fca0c806d089d618a79be13a6cd2a2d4cb911388ab7" -->
<!-- pdac:cite id="TERM-VAT-SCHEME" digest="sha256:d7fbf3cf62480bae491180a199e8fbe4455b25712d349af2b203173abf42095f" -->
<!-- pdac:cite id="BR-FIN-USED-RATE-FROZEN" digest="sha256:b33134bd0f2519dfb830cd37b214eab8e6c9e8609b7a9bf63c9db8e2e6cb8a95" -->
