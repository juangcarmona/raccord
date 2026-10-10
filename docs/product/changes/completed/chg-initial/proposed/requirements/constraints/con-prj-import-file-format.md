---
id: CON-PRJ-IMPORT-FILE-FORMAT
type: constraint
title: "Bulk import file format"
status: draft
applies-to:
  - "UC-PRJ-IMPORT-CONTACTS"
  - "UC-PRJ-IMPORT-TASKS"
  - "UC-PRJ-IMPORT-EQUIPMENT-LIST"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360013553000.md, .private/research/help-center/articles/39225660180626.md, .private/research/help-center/articles/4413562661138.md"
  confidence: high
  recovered-from: documentation
---

## Constraint

Bulk imports (contacts, tasks, equipment lists) accept spreadsheet (.xlsx) or comma-separated files with a single sheet, at most 64 columns, plain unformatted text values, no embedded images, and an optional header row; the preview before import shows only the first ten rows.

## Rationale

Deliberately fixed limits of the import facility, which processes files of bounded size and structure.

## Consequences

Files with several sheets, formatting, images or more than 64 columns must be prepared before import; very large files must be split.
