---
id: FR-PRJ-034
type: functional-requirement
title: "Import contacts"
status: draft
derived-from:
  - "UC-PRJ-IMPORT-CONTACTS"
  - "BR-PRJ-CONTACT-IMPORT-MODES"
  - "BR-PRJ-CONTACT-PERSON-IMPORT-ROWS"
  - "BR-PRJ-IMPORT-FOLDER-PLACEMENT"
  - "CON-PRJ-IMPORT-FILE-FORMAT"
verification:
  - scenario-ref: "SB-PRJ-CONTACT-IMPORT-SKIP-EXISTING"
  - scenario-ref: "SB-PRJ-IMPORT-FAILED-ROWS-FILE"
uses-terms:
  - "TERM-CONTACT"
provenance:
  source: ".private/research/help-center/articles/360013553000.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST import contacts and contact persons in create, update or skip-existing mode, placing them by folder path, tagging the import and reporting failed rows.

## Rationale

Fast database set-up.
