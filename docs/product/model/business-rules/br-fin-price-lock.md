---
id: BR-FIN-PRICE-LOCK
type: business-rule
title: "A locked project price ignores plan changes"
status: draft
applies-to:
  - "UC-FIN-LOCK-PROJECT-PRICE"
uses-terms:
  - "TERM-PRICE-LOCK"
provenance:
  source: ".private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Rule

While a project's price is locked, changes to planned equipment, crew and transport do not change the prices, discounts or totals of the financial overview; unlocking recalculates them from the plan.

## Rationale

Agreed prices must survive late operational changes.

## Examples

See SB-FIN-LOCKED-PRICE-UNCHANGED.

## Exceptions

None.
