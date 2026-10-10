---
id: BR-FIN-PLANNED-COST-SOURCES
type: business-rule
title: "Planned costs come from the resources actually scheduled"
status: draft
applies-to:
  - "UC-FIN-REVIEW-PROJECT-FINANCIALS"
uses-terms:
  - "TERM-PLANNED-COST"
provenance:
  source: ".private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Planned crew cost of a scheduled person is the selected crew rate applied to the function length minus breaks, or the price agreed in the function when no rate is set; planned transport cost uses the fixed and variable rate selected for each assigned vehicle (the vehicle's standard rate unless changed).

## Rationale

The plan becomes more precise as people and vehicles are chosen.

## Examples

A scheduled crew member at 30.00 per hour on a four-hour function without breaks gives a planned crew cost of 120.00.

## Exceptions

Sale equipment uses its purchase price (BR-FIN-SALE-ITEM-COST).
