---
id: BR-EQP-COUNT-COMPLETION
type: business-rule
title: "Completing a count applies differences"
status: draft
applies-to:
  - "UC-EQP-COUNT-SELECTED-EQUIPMENT"
uses-terms:
  - "TERM-COUNT-RESULT"
  - "TERM-INVENTORY-COUNT"
  - "TERM-LOST-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360015960120.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Each count line is correct, short or in surplus by comparison with expected stock; completing the count applies the differences to stock as stock movements and records missing units as lost equipment.

## Rationale

Counting must lead to corrected records.

## Examples

- Expected 4, counted 3 cables: completing reduces stock by 1 and records 1 lost.

## Exceptions

None.
