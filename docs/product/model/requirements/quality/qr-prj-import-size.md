---
id: QR-PRJ-IMPORT-SIZE
type: quality-requirement
title: "Import column limit"
status: draft
quality-attribute: "capacity"
applies-to:
  - "UC-PRJ-IMPORT-CONTACTS"
  - "UC-PRJ-IMPORT-TASKS"
verification:
  - scenario: "A 64-column contact file imports; a 65-column file is refused"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360013553000.md, .private/research/help-center/articles/39225660180626.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

Bulk imports MUST accept files of up to 64 columns.

## Measurement

Import a file with 64 columns: accepted. A file with 65 columns: rejected or the user is told to reduce columns.
