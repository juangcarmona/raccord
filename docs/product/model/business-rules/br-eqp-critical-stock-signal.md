---
id: BR-EQP-CRITICAL-STOCK-SIGNAL
type: business-rule
title: "Critical stock flags replenishment"
status: draft
applies-to:
  - "UC-EQP-CREATE-SALE-ITEM"
uses-terms:
  - "TERM-CRITICAL-STOCK"
provenance:
  source: ".private/research/help-center/articles/360013812939.md, .private/research/help-center/articles/13021031495698.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

A tracked sale item whose stock is at or below its critical stock level can be selected with a catalog filter for replenishment; reaching the level blocks nothing.

## Rationale

A reorder aid, not a constraint.

## Examples

- Haze fluid with critical stock 5 and stock 4 appears in the critical stock filter.

## Exceptions

None.
