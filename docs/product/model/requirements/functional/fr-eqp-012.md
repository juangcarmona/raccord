---
id: FR-EQP-012
type: functional-requirement
title: "Import and update equipment from files"
status: draft
derived-from:
  - "UC-EQP-IMPORT-EQUIPMENT"
  - "UC-EQP-MASS-UPDATE-EQUIPMENT"
  - "BR-EQP-IMPORT-MATCH-UNIQUE"
  - "BR-EQP-IMPORT-DUPLICATE-CODES"
  - "CON-EQP-IMPORT-FILE-FORMAT"
verification:
  - scenario-ref: "SB-EQP-IMPORT-DUPLICATE-CODE"
  - scenario-ref: "SB-EQP-IMPORT-LONG-CODE"
  - scenario-ref: "SB-EQP-UPDATE-MATCH-ON-NAME"
uses-terms:
  - "TERM-EQUIPMENT-IMPORT"
  - "TERM-IMPORT-MODE"
  - "TERM-IMPORT-MATCH-COLUMN"
provenance:
  source: ".private/research/help-center/articles/360014141299.md, .private/research/help-center/articles/360014293220.md, .private/research/help-center/articles/13725992768786.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST import equipment items from spreadsheet or CSV files in all-new, skip-existing and update modes, with column mapping including custom fields, root folder placement, match columns, a 10-row preview, an import tag and a result report of failed rows with reasons.

## Rationale

Bulk onboarding and bulk changes.
