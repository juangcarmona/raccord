---
id: BR-WHS-PACKING-ALTERNATIVE-PRICE
type: business-rule
title: "Alternatives chosen while packing keep the planned price"
status: draft
applies-to:
  - "UC-WHS-SWAP-ALTERNATIVE-PACKING"
uses-terms:
  - "TERM-EQUIPMENT-ALTERNATIVE"
provenance:
  source: ".private/research/help-center/articles/13901986840082.md, .private/research/help-center/articles/14074151954834.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An alternative chosen during packing replaces the planned line at the planned line's price, except inside a virtual combination where the alternative gets a zero price; price changes are done in the project afterwards.

## Rationale

The warehouse must not change commercial terms.

## Examples

Swapping a console during packing keeps the quoted price.

## Exceptions

None.
