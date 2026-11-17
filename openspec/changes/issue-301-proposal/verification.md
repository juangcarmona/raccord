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

## Remaining integration gates

The existing PR must publish the runtime candidate, pass every required check on its actual head, and receive the final integration review before merge. Local execution, rendered review, forge checks and merge are separate receipts; tasks 5.2 and 5.3 remain open until those steps are complete.
