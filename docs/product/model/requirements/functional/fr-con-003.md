---
id: FR-CON-003
type: functional-requirement
title: Import contacts with row validation and audit
status: active
derived-from:
  - UC-CON-IMPORT-CONTACTS
  - BR-CON-IMPORT-ROW-ATOMICITY
  - BR-CON-IMPORT-AUDIT
verification:
  - scenario-ref: SB-CON-IMPORT-VALID-AND-INVALID-ROWS
  - scenario-ref: SB-CON-IMPORT-RECHECKS-PREVIEW
uses-terms:
  - TERM-CONTACT
---

## Requirement

The product MUST allow an authorized user to map, preview and validate the supported file defined by CON-CON-IMPORT-FILE-FORMAT, then confirm create-only import. Confirmation MUST verify the previewed file digest and validate again before writes. Each valid row MUST create one complete contact atomically; invalid rows MUST be reported with row numbers and reasons without preventing valid rows from succeeding. BR-CON-IMPORT-AUDIT governs audit metadata and privacy. Updates, skip-existing, contact persons, import-time tags/folders/custom fields, undo and downloadable rejected-row files are outside this profile.

## Rationale

Users can set up the directory in bulk while preventing incomplete records and retaining a minimal audit trail.
