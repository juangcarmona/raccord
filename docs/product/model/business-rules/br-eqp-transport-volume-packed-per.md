---
id: BR-EQP-TRANSPORT-VOLUME-PACKED-PER
type: business-rule
title: "Transport volume by package"
status: draft
applies-to:
  - "UC-EQP-CONFIGURE-ITEM-DETAILS"
uses-terms:
  - "TERM-PACKED-PER"
  - "TERM-TRANSPORT-VOLUME"
provenance:
  source: ".private/research/help-center/articles/13141562920338.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The transport volume of a planned quantity of an item with a packed-per quantity equals the item's volume per package multiplied by the planned quantity divided by the packed-per quantity, rounded up; this never changes the quantity taken from stock.

## Rationale

Goods travel in whole crates even when partially filled.

## Examples

- Glasses packed per 10: planning 11 counts two crates of volume, while only 11 glasses are deducted from availability.

## Exceptions

None.
