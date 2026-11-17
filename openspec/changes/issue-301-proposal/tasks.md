---
pdac-scope: cited
---

# Tasks

## 1. Prerequisites and source recovery

- [x] 1.1 Confirm #443 corrected-head readiness and presence in the actual base, and #437 serial activation; record the qualifying revisions and decision before implementation.
- [x] 1.2 Compare supplied historical source packets against that base; document reusable deltas and retain current designation and transaction changes, verifying no whole-file host or model restoration.

## 2. Parser and validation

- [x] 2.1 Adapt CSV and single-sheet XLSX parsers and test optional headers, quoting, 10 MiB and 64-column boundaries, formulas, images, unsupported types, malformed archives and mappings.
- [x] 2.2 Preserve sparse row numbers without gap allocations; test malformed, overflowing and inconsistent references and archive/XML resource limits without a product row cap.
- [x] 2.3 Reuse Contacts creation validation and test all-row results, mapping validity, header selection and first-10 sample without writes.
<!-- pdac:cite id="CON-CON-IMPORT-FILE-FORMAT" digest="sha256:f33969383814d4ba04d00eadd559a0dd470c67958d33cfc42ff6a8aded62f9fb" -->

## 3. Application, persistence and API

- [x] 3.1 Add permission-gated inspect, preview and confirm contracts; real API tests prove digest mismatch, invalid mapping and revalidation after preview cause the specified no-write or row-rejection outcomes.
- [x] 3.2 Implement independent row transactions and expected-conflict recovery; real database/API tests inject a late expected conflict and prove rollback, no row audit residue, and later valid-row success. Test cancellation and database failure propagate.
- [x] 3.3 Record minimal start and completed audit outcomes; API tests assert actor, server time, import id, file type, counts and no uploaded bytes or values in audit or logs, including all-invalid input.
- [x] 3.4 Test authorization on every endpoint and separate installation/database isolation; regenerate OpenAPI and typed client against the actual base and verify drift is empty.
<!-- pdac:cite id="BR-CON-IMPORT-ROW-ATOMICITY" digest="sha256:f6c233e920161353d0bdac7a81156a1fc58dca933546a8da314593dc4b61b2b8" -->
<!-- pdac:cite id="BR-CON-IMPORT-AUDIT" digest="sha256:bfa968766a4f0c22c180c9c6bad1742062be10f37b375f15c6e5058aded4327f" -->

## 4. UI and journey

- [x] 4.1 Adapt authenticated Contacts import UI with accessible mapping, preview, confirmation and rejection report; frontend tests cover errors, file/header/mapping preview invalidation and removing ignored mappings.
- [x] 4.2 Extend `tests/Raccord.EndToEndTests/Customers/ContactImportJourney_Should_ImportValidRowsAndReportRejections.cs` for three valid and one invalid synthetic row, persistence, rejection and privacy; preserve existing `tests/Raccord.EndToEndTests/per-pr-selection.txt` entries and add the `Area=contacts` journey.
- [x] 4.3 Capture synthetic rendered evidence at supported viewports and appearances; review against `docs/design/DESIGN.md` using scoped Taste fallback, keyboard, focus, contrast and narrow layout checks; record findings and required human acceptance.
<!-- pdac:cite id="SB-CON-IMPORT-VALID-AND-INVALID-ROWS" digest="sha256:433ee6a37d8cfa638c74c8066aabcad2980c9b7908d2714c01cf1f24ad1c8454" -->
<!-- pdac:cite id="SB-CON-IMPORT-RECHECKS-PREVIEW" digest="sha256:e206d5bdd26ce6f69423f7fd81fe5b2ac2c3faae55d21e0abad07407f53ba122" -->

## 5. Integration evidence

- [x] 5.1 Refresh affected citations from the current provider and verify all citations and strict OpenSpec validation on the final source/model revisions.
- [ ] 5.2 Run repository CI-equivalent build/tests, CSharpier, Prettier, lint-staged, commitlint, contract drift and selected authenticated Aspire browser journey; record exact commands, run IDs, revisions, exit statuses and results.
- [ ] 5.3 Prepare behaviour-to-test evidence with current citations and separate execution from reviewer acceptance; obtain required review before integration.

## Source recovery evidence

PR #443 qualified head `0eb0a0ed64a478871ecc5fe38066f0a42fd508c8` passed native run 43996485359 and current-head CI 44001898757, then merged as `166fd329cc8f312137ecc0abb81052a1ee8793dc`, verified in main before #301 activation. Existing proposal branch integrates that commit through `62d567e7b3f6a5af4230bd2f9c5a61792745ffb5`. Applied only 28 immutable source/test/architecture deltas from historical base `8be3215e894a721fd09fc521ebb6216ba739df80` to `6486327b3db5dda4062a79ba77e4745cd497fea9`; generated contracts require regeneration, and no canonical model, whole host or designation transaction file was restored. Current transaction rollback and sparse XLSX defects remain explicit implementation work.

Local qualification: 244 backend tests, 53 frontend tests, lint/build and the real three-valid/one-rejected import browser journey passed. The manifest has 39 journeys. `verification.md` maps behavior to tests and records synthetic viewport/appearance review; full published-head CI and final integration review remain pending under tasks 5.2 and 5.3.
