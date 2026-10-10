---
id: SB-EQP-VIRTUAL-KIT-STOCK-LIMITED
type: structured-behaviour
title: "Virtual kit stock limited by scarcest content"
status: draft
illustrates:
  - "BR-EQP-VIRTUAL-KIT-STOCK-DERIVED"
given:
  - "A virtual kit needs 1 of item A, 3 of item B and 4 of item C"
  - "Item A has stock 10, item B 12 and item C 100"
when: "The stock of the virtual kit is computed"
then:
  - "The kit's stock is 4"
  - "Item B is reported as the limiting content"
uses-terms:
  - "TERM-VIRTUAL-KIT"
  - "TERM-STOCK-QUANTITY"
provenance:
  source: ".private/research/help-center/articles/4402870209682.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Fixes the floor-of-minimum-ratio calculation of derived kit stock.

## Boundaries

Does not cover availability over a period, which uses unreserved quantities.
