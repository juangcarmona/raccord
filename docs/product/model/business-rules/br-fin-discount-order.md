---
id: BR-FIN-DISCOUNT-ORDER
type: business-rule
title: "Discounts apply from line to category to total"
status: draft
applies-to:
  - "UC-FIN-APPLY-PROJECT-DISCOUNTS"
uses-terms:
  - "TERM-DISCOUNT-LEVEL"
provenance:
  source: ".private/research/help-center/articles/360013264720.md, .private/research/help-center/articles/360013886600.md, .private/research/help-center/articles/360013890780.md, .private/research/help-center/articles/360013944139.md"
  confidence: high
  recovered-from: interview
---

## Rule

Price factors and line discounts (including discount-group percentages) determine line totals; category discounts apply to the sum of those line totals; the project discount applies last to the sum of the discounted categories.

## Rationale

Each level reduces the result of the narrower one, so levels never double-count the same base.

## Examples

Line 1,000 with 10% line discount gives 900; a 10% rental category discount gives 810; a 5% total discount gives 769.50.

## Exceptions

The insurance charge is computed on the gross rental price before any discount (BR-FIN-INSURANCE-ON-GROSS-RENTAL).
