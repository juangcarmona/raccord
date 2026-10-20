---
id: CHG-CONTACT-SPREADSHEET-IMPORT-301
type: product-change
title: Accept validated spreadsheet import for contacts
status: proposed
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

- Which file types and structural limits are supported? Recommendation: accept CSV and XLSX with a single sheet, at most 64 columns, plain text values, no embedded images, optional header row, and a preview of the first 10 data rows, consistent with the existing draft format constraint. No row-count limit is proposed pending product/operations input.
- Should the import complete valid rows when some rows are rejected, with atomicity per row? Recommendation: yes, because #301 explicitly requires valid rows to import while invalid rows are reported.
- Should administrators download a rejected-row report, or is an in-product list sufficient? Recommendation: provide an in-product list with row number and reason for this story; downloadable correction/re-import is a separate follow-up.
- Confirm that this story creates new contacts only and excludes contact-person rows, update/skip matching, folder placement, tags, custom fields, and import undo. Recommendation: yes; these behaviors are outside #301 and depend on other contact issues.

## Product Acceptance

- The Product Owner confirms supported file types and structural limits.
- The Product Owner confirms valid-row import with per-row atomicity when other rows are rejected.
- The Product Owner confirms the in-product rejected-row report and the new-contact-only scope.
- The Product Owner confirms the audit record contains actor, time and outcome summary but no retained upload or raw contact values.

## Out of Scope

Updating or skipping existing contacts, importing contact persons, folder placement, tags, custom fields, undo, downloadable correction files, other record types, registry lookup, production data, and implementation, technical design, release or deployment.
