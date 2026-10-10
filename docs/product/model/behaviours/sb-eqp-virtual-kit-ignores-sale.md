---
id: SB-EQP-VIRTUAL-KIT-IGNORES-SALE
type: structured-behaviour
title: "Sale content does not limit a virtual kit"
status: draft
illustrates:
  - "BR-EQP-VIRTUAL-KIT-STOCK-DERIVED"
given:
  - "A virtual kit contains two rental items with ample availability and one sale item with zero stock"
when: "Availability of the virtual kit is computed"
then:
  - "The sale item is ignored in the kit's availability"
  - "The sale item's own availability is still calculated separately"
uses-terms:
  - "TERM-VIRTUAL-KIT"
  - "TERM-SALE-ITEM"
provenance:
  source: ".private/research/help-center/articles/4402870209682.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Prevents consumables from blocking a planning set.

## Boundaries

None.
