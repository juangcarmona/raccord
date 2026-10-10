---
id: BR-WHS-ALTERNATIVE-REPLACEMENT
type: business-rule
title: "Replacing by an alternative chooses quantity and price policy"
status: draft
applies-to:
  - "UC-WHS-SOLVE-WITH-ALTERNATIVE"
uses-terms:
  - "TERM-EQUIPMENT-ALTERNATIVE"
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/16611399161234.md, .private/research/help-center/articles/360017700639.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Solving a shortage with an alternative replaces a chosen number of units (only the short units, or all) by the alternative article, and the planner chooses whether the replaced quantity keeps the original line price or takes the alternative's price.

## Rationale

The customer was quoted a price; swapping gear must not silently change it.

## Examples

See SB-WHS-ALTERNATIVE-KEEPS-PRICE.

## Exceptions

None.
