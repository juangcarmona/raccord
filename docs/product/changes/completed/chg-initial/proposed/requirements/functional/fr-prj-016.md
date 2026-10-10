---
id: FR-PRJ-016
type: functional-requirement
title: "Import equipment lists into projects"
status: draft
derived-from:
  - "UC-PRJ-IMPORT-EQUIPMENT-LIST"
  - "BR-PRJ-EQUIPMENT-IMPORT-MODES"
  - "BR-PRJ-EQUIPMENT-IMPORT-MATCHING"
  - "BR-PRJ-EQUIPMENT-IMPORT-PLACEMENT"
  - "CON-PRJ-IMPORT-FILE-FORMAT"
verification:
  - scenario-ref: "SB-PRJ-IMPORT-NO-ACCESSORIES"
  - scenario-ref: "SB-PRJ-IMPORT-FAILED-ROWS-FILE"
uses-terms:
  - "TERM-SAVED-IMPORT-MATCH"
provenance:
  source: ".private/research/help-center/articles/4413562661138.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST import equipment lists into a project in append or replace mode, matching rows to items (with saved matches and temporary items), planning them in an imported-equipment group and reporting failed rows.

## Rationale

Avoids retyping design-tool output.
