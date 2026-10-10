---
id: "BR-DOC-PRICE-IN-PROJECT"
type: business-rule
title: "Line amounts on documents come from the price in project"
status: draft
applies-to:
  - "UC-DOC-GENERATE-DOCUMENT"
uses-terms:
  - "TERM-ADDITIONAL-COSTS-MODULE"
  - "TERM-ADDITIONAL-COST"
provenance:
  source: ".private/research/help-center/articles/27353381539602.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Document totals of additional costs are always calculated from quantity, sale price and discount; the amount shown per line is the price in project, which is consistent with those totals, while the sale price is the unit price before quantity and discount.

## Rationale

Documents that print only the sale price mislead customers once quantities or discounts are used.

## Examples

See SB-DOC-ADDITIONAL-COSTS-SALE-PRICE-MISMATCH.

## Exceptions

When the quantity is 1 and there is no discount, sale price and price in project are equal.
