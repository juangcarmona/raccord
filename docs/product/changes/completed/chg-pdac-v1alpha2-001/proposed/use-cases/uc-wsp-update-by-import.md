---
id: UC-WSP-UPDATE-BY-IMPORT
type: use-case
title: "Update or extend records by re-importing an export"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-RECORD-IDENTIFIER-IMMUTABLE"
  - "BR-WSP-EMPTY-IDENTIFIER-CREATES"
  - "BR-WSP-EDITED-IDENTIFIER-MISMATCH"
  - "BR-WSP-BACKUP-SCHEDULE"
uses-terms:
  - "TERM-RECORD-IDENTIFIER"
  - "TERM-IMPORT-MATCH-COLUMN"
provenance:
  source: ".private/research/help-center/articles/36084108749842.md, .private/research/help-center/articles/36811784028178.md, .private/research/help-center/articles/36812030570770.md"
  confidence: medium
  recovered-from: documentation
---

## Goal

Many records are corrected or added through a spreadsheet without touching them one by one.

## Trigger

A broad correction (for example outdated prices) or a stock write-off of sold equipment is needed.

## Preconditions

- The user may export and import the records concerned.

## Main Flow

1. The user exports the records including the identifier column and the columns to change.
2. The user edits values in a spreadsheet, removes unneeded columns to stay within the column limit, removes formulas and saves as a delimited file.
3. The user imports the file; the product recognises the identifier column as match key.
4. The product updates matched records, creates rows without identifier as new records and reports the result.

## Alternative Flows

- Bulk stock reduction after a sale: quantities are recalculated in the sheet and imported.

## Failure Conditions

- Too many columns, formulas or duplicate scan codes: the import fails and reports why.

## Postconditions

- Records reflect the file; a nightly backup allows rollback if needed.
