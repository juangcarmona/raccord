---
id: CHG-CONTACT-SPREADSHEET-IMPORT-301
type: product-change
title: Accept validated spreadsheet import for contacts
status: applied
base-revision: "769edf87e11dec97b23de000741c2f745545256b"
operations:
  add:
    - UC-CON-IMPORT-CONTACTS
    - FR-CON-003
    - BR-CON-IMPORT-ROW-ATOMICITY
    - BR-CON-IMPORT-AUDIT
    - CON-CON-IMPORT-FILE-FORMAT
    - SB-CON-IMPORT-VALID-AND-INVALID-ROWS
  modify: []
  remove: []
---

## Problem

Issue #301 requires administrators to load contacts from a spreadsheet after reviewing mapped fields and row validation. Invalid records must not be partially saved, valid records must be importable, and the operation must be auditable. Existing draft imports also include updates, contact persons, folders and tags; those behaviors exceed this issue's scope and depend on later contact stories.

## Intended Product Outcome

An authorized workspace administrator uploads a supported spreadsheet, maps columns to accepted contact fields, reviews a preview and row-level validation results, and imports the valid rows. Each row is saved in full or not saved. Valid rows may be imported even when other rows fail. The result identifies rejected rows and reasons. The audit trail records who performed the import, when, and its outcome without retaining the uploaded file or its contact values.

## Rationale

Row-level atomicity lets teams recover rejected rows without creating incomplete contacts, while an outcome record supports accountability without retaining extra contact data.

## Affected Product Areas

Contacts directory creation, spreadsheet upload, column mapping, validation feedback, and audit history.

## Open Questions

None. This change accepts CSV and XLSX files with one worksheet, up to 64 columns, plain text values, no embedded images, an optional header row, and a preview of the first 10 data rows. It imports each valid row atomically and reports rejected row numbers and reasons in the product. It creates new contacts only and does not retain uploaded files or raw contact values for audit.

## Product Acceptance

- Accept CSV and XLSX uploads with one worksheet, up to 64 columns, plain text values, no embedded images, an optional header row, and a 10-data-row preview.
- Import valid rows even when other rows are rejected; persist or reject each row atomically.
- Show rejected row numbers and validation reasons in the product; downloadable correction files and re-import are outside this change.
- Create new contacts only; do not update or skip existing records, import contact persons, or assign folders, tags, or custom fields.
- Record actor, time, and outcome counts for audit without retaining the uploaded file or raw contact values.
- Limit each upload to 10 MiB to bound temporary-file and parser resource use; this technical limit does not impose a row-count limit.

## Out of Scope

Updating or skipping existing contacts, importing contact persons, folder placement, tags, custom fields, undo, downloadable correction files, other record types, registry lookup, production data, and implementation, technical design, release or deployment.
