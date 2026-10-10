---
id: "FR-TRB-011"
type: functional-requirement
title: "Warehouse board shows only confirmed, plannable work"
status: draft
derived-from:
  - "BR-WHS-BOARD-DUE-DATE"
verification:
  - scenario-ref: "SB-TRB-UNCONFIRMED-NOT-ON-BOARD"
  - scenario-ref: "SB-TRB-BOARD-DATE-CHANGED"
uses-terms:
  - "TERM-PACKING-LIST"
provenance:
  source: ".private/research/help-center/articles/360015018360.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST list in the to-pack section of the warehouse board only confirmed projects whose due date is the selected date.

## Rationale

The warehouse must only pack real, confirmed work.
