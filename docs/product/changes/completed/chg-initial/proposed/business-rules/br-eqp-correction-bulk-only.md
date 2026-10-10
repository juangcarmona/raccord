---
id: BR-EQP-CORRECTION-BULK-ONLY
type: business-rule
title: "Stock corrections apply to tracked stock"
status: draft
applies-to:
  - "UC-EQP-CORRECT-STOCK"
uses-terms:
  - "TERM-STOCK-CORRECTION"
provenance:
  source: ".private/research/help-center/articles/360015957259.md, .private/research/help-center/articles/360013812939.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A stock correction to a stated new level can be made for any item that has its own stock, including tracked sale items; the stock of a virtual kit is derived from its content and cannot be corrected.

## Rationale

Recorded stock must be able to follow the physical stock.

## Examples

- Correcting cable stock from 52 to 50 records minus 2.

## Exceptions

None.
