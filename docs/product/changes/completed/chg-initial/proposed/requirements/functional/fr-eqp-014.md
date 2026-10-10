---
id: FR-EQP-014
type: functional-requirement
title: "Manage stock movements"
status: draft
derived-from:
  - "UC-EQP-SCHEDULE-STOCK-MOVEMENT"
  - "UC-EQP-CORRECT-STOCK"
  - "BR-EQP-MOVEMENT-EFFECTIVE-DATE"
  - "BR-EQP-CORRECTION-BULK-ONLY"
verification:
  - scenario-ref: "SB-EQP-SCHEDULED-DELIVERY"
  - scenario: "Correcting an item's stock from 52 to 50 records a movement of minus 2 with its description."
uses-terms:
  - "TERM-STOCK-MOVEMENT"
  - "TERM-SCHEDULED-STOCK-MOVEMENT"
  - "TERM-STOCK-CORRECTION"
provenance:
  source: ".private/research/help-center/articles/16459216697106.md, .private/research/help-center/articles/360015957259.md, .private/research/help-center/articles/360013812939.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let users enter dated deliveries, reductions and corrections for items and tracked sale items, and MUST apply them to stock from their date.

## Rationale

Stock must be kept accurate over time.
