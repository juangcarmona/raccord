---
pdac-scope: cited
---

# Contact import verification

## Behavior evidence

The implementation consumes the accepted contact import baseline; no canonical product artifact or schema migration was changed.
<!-- pdac:cite id="BR-CON-IMPORT-ROW-ATOMICITY" digest="sha256:f6c233e920161353d0bdac7a81156a1fc58dca933546a8da314593dc4b61b2b8" -->

| Behavior | Executable evidence |
| --- | --- |
| CSV, optional headers, quoted fields, 10 MiB and 64-column boundaries | `ContactSpreadsheetParserTests` |
| Plain-text single-sheet XLSX, forbidden formulas/images/macros, malformed archives, 40 MiB expanded bound | `ContactSpreadsheetParserTests` |
| Sparse original row numbers, malformed/overflowing/inconsistent references, no blank-gap allocation | `Contact_import_Should_PreserveSparseOriginalRowNumbersWithoutAllocatingGaps`, reference theories |
| All-row validation with a ten-row sample and no writes | `Contacts_import_Should_ValidateEveryRowBeyondTheTenRowSampleWithoutWrites` |
| Changed digest, invalid mapping, confirmation revalidation | `ContactsDirectoryTests` import rejection and revalidation cases |
| Late database conflict, complete rollback, later-row success | `Contacts_import_Should_RollBackLateConflictAndContinueWithLaterRows` |
| Cancellation and infrastructure errors propagate | `ContactsDirectoryTests` propagation cases |
| Minimal actor/time/type/count audit and isolation | Import audit assertions and `Contacts_import_Should_LeaveOtherInstallationContactsAndAuditUnchanged` |
| Create permission on inspection, preview and confirmation | Three endpoint cases of `Contacts_import_Should_RequireContactCreatePermission` |
| File/header/mapping changes invalidate preview; ignore removes a mapping; upload errors retain explanations | `ContactImportPage.test.tsx` |
| Three accepted contacts, one rejected row, persisted results and privacy-preserving audit | `ContactImportJourney_Should_ImportValidRowsAndReportRejections` |

## Local results

- `dotnet test Raccord.slnx -c Release --no-restore -m:1 --filter FullyQualifiedName!~Raccord.EndToEndTests`: 244 passing backend tests, zero failed or skipped. The filter intentionally excludes the separate browser assembly.
- Frontend: 53 passing tests, lint and build passed. OpenAPI and the typed client were regenerated from the implemented host.
- The import browser journey passed against the real authenticated Aspire application with three valid and one invalid synthetic row. The committed selection check passes with 39 journeys; this single-journey local run does not replace full selected-suite CI.
- Product model: 1,918 artifacts with zero diagnostics. Product citations, strict OpenSpec and Markdown checks passed; CI must verify the published head independently.

## Rendered review

The import journey captures the preview at 1280 and 390 pixels in both light and dark appearances using the repository's existing color-scheme and `data-appearance` mechanisms. It verifies no horizontal page overflow and keyboard movement from the type selector to the legal-name selector. All four synthetic captures were reviewed by the Codex operator against `docs/design/DESIGN.md` using the scoped manual fallback: aligned field grid, bounded horizontally scrolling tables, visible focus, textual counts and validation, and existing semantic text/surface tokens. A narrow file-input overflow was reproduced and corrected before the passing run.

The capture hashes and operator receipt are retained outside Git with the delivery-watch qualification evidence; screenshots contain synthetic records only. This operator review makes no separate claim of human visual acceptance. Existing owner delivery and merge authorization is recorded in #437.

## Published qualification and integration review

Source candidate `57107f3b96658de4ac31ee8b431b752912afb05d` passed [full CI 44007623177](https://github.com/juangcarmona/raccord/actions/runs/44007623177), [guardrails 44007623136](https://github.com/juangcarmona/raccord/actions/runs/44007623136) and [engineering validation 44007623130](https://github.com/juangcarmona/raccord/actions/runs/44007623130). Actual selected browser output reports 41 cases, zero errors, failures, skips or not-run cases, in 187.363 seconds; the separate metadata check also passed both cases. Security, dependency/license, image/Compose/Bicep, product, OpenSpec, formatting and contract-drift gates passed.

The Codex operator reviewed the implemented boundaries against the applied baseline and all fifteen tasks: bounded parser inputs, original sparse row numbering, complete row rollback and persistence-context recovery, propagation of unexpected/cancelled operations, authorization on every endpoint, separate-installation data and audit isolation, uploaded-value privacy, input-change invalidation, error explanations and preservation of the integrated designation/snapshot behavior. The generic concurrency exception retains its HTTP mapping after moving to SharedKernel; no dependency, migration or product-policy change was introduced.

This final documentation update records completed source qualification and review. It leaves the runtime unchanged; checks on its own published head must still pass before the owner-authorized merge. No additional human acceptance or production deployment is claimed.
