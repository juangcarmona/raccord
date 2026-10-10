---
id: BR-EQP-STOCK-INFLUENCES
type: business-rule
title: "What changes stock"
status: draft
applies-to:
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-STOCK-QUANTITY"
  - "TERM-STOCK-MOVEMENT"
  - "TERM-LOST-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360015957259.md, .private/research/help-center/articles/360015960120.md, .private/research/help-center/articles/16459216697106.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Stock of an item changes only through manual stock movements (deliveries, reductions, corrections), imports, completed inventory counts, loss and found reports, and for sale items warehouse bookings.

## Rationale

Stock must be traceable to explicit events.

## Examples

- Planning 20 lights on a project changes availability but not stock.

## Exceptions

None.
