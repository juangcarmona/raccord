---
id: BR-EQP-MARGIN-INFORMATIONAL
type: business-rule
title: "Margin price is informational"
status: draft
applies-to:
  - "UC-EQP-CONFIGURE-ITEM-DETAILS"
uses-terms:
  - "TERM-MARGIN-PRICE"
provenance:
  source: ".private/research/help-center/articles/13141562920338.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The margin price of an item is stored and displayed only; no price, cost or total in the product uses it.

## Rationale

It is an internal target, not a calculation input.

## Examples

- Changing the margin price changes no quote.

## Exceptions

None.
