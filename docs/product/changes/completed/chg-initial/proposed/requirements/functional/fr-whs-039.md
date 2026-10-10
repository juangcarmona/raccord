---
id: FR-WHS-039
type: functional-requirement
title: "Scan codes"
status: draft
derived-from:
  - "UC-WHS-ASSIGN-SCAN-CODES"
  - "UC-WHS-PRINT-CODE-LABELS"
  - "BR-WHS-SCAN-CODE-UNIQUE"
  - "CON-WHS-CODE-LENGTH"
verification:
  - scenario-ref: "SB-WHS-LONG-CODE-TRUNCATED"
uses-terms:
  - "TERM-SCAN-CODE"
provenance:
  source: ".private/research/help-center/articles/360013101260.md, .private/research/help-center/articles/36515434639122.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST generate or accept unique codes for articles and cases and print code labels.

## Rationale

Identification.
