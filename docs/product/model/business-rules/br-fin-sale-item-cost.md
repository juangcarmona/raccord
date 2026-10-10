---
id: BR-FIN-SALE-ITEM-COST
type: business-rule
title: "Cost of sold equipment is purchase price times quantity"
status: draft
applies-to:
  - "UC-FIN-REVIEW-PROJECT-FINANCIALS"
uses-terms:
  - "TERM-ESTIMATED-COST"
  - "TERM-PLANNED-COST"
provenance:
  source: ".private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Equipment sold on a project costs its purchase price times the quantity, and that amount appears identically as estimated and planned cost.

## Rationale

Sale items have a known cost from the start.

## Examples

Five cables bought at 4.00 each give 20.00 estimated and 20.00 planned cost.

## Exceptions

None.
