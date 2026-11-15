---
id: UC-CRW-IMPORT-CREW-MEMBERS
type: use-case
title: "Import crew members from a file"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-IMPORT-EXCLUSIONS"
  - "BR-CRW-IMPORT-MATCH-COLUMN"
  - "BR-CRW-IMPORT-FOLDER-PLACEMENT"
  - "BR-CRW-IMPORT-HEADER-ROW"
  - "BR-CRW-IMPORT-TAG"
  - "BR-CRW-IMPORT-ROW-RESULT"
  - "BR-CRW-ACCOUNT-INVITATION-ON-CREATION"
uses-terms:
  - "TERM-RESOURCE-IMPORT"
  - "TERM-CREW-MEMBER"
provenance:
  source: ".private/research/help-center/articles/360014366299.md, .private/research/help-center/articles/360014360520.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Many crew members are created or updated at once.

## Trigger

The installation is set up or many records change.

## Preconditions

A single-sheet spreadsheet or delimited file with plain-text values and at most 64 columns.

## Main Flow

1. The planner chooses the import mode: all rows new, update existing, or add new and skip existing.
2. The planner uploads the file and states whether it has a header row.
3. The product proposes a mapping of columns to crew fields (including custom fields); the planner adjusts it.
4. The planner selects the root folder.
5. For update or skip modes the planner selects match columns.
6. The product shows a preview of the first 10 rows.
7. The planner confirms; the product imports and reports the result.

## Alternative Flows

- Login permission column set to 0: no account invitations go out.
- The import went partly wrong: the planner deletes all records with the import tag and imports again in smaller files.

## Failure Conditions

Files too large, formatted values or multiple sheets make rows or the whole import fail; failed rows are listed with reasons.

## Postconditions

Imported crew have the default user role, the import tag and their folder.
