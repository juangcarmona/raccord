---
id: FR-EQP-010
type: functional-requirement
title: "Apply accessory options when planning"
status: draft
derived-from:
  - "UC-EQP-CONFIGURE-ACCESSORIES"
  - "BR-EQP-ACCESSORY-OPTIONS"
  - "BR-EQP-ACCESSORY-DETACHED-WHEN-PLANNED"
verification:
  - scenario-ref: "SB-EQP-ACCESSORY-PROMPT"
  - scenario-ref: "SB-EQP-ACCESSORY-SKIP-PRESENT"
  - scenario-ref: "SB-EQP-ACCESSORY-MERGE"
uses-terms:
  - "TERM-ACCESSORY"
provenance:
  source: ".private/research/help-center/articles/360013703140.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST, when an item is planned, add or offer its accessories according to their automatic, skip-if-present, new-line and free options, and MUST then treat them as independent lines.

## Rationale

Faster, more complete planning.
