---
id: BR-EQP-MOVEMENT-EFFECTIVE-DATE
type: business-rule
title: "Stock movements take effect on their date"
status: draft
applies-to:
  - "UC-EQP-SCHEDULE-STOCK-MOVEMENT"
uses-terms:
  - "TERM-SCHEDULED-STOCK-MOVEMENT"
  - "TERM-STOCK-QUANTITY"
provenance:
  source: ".private/research/help-center/articles/16459216697106.md, .private/research/help-center/articles/360015957259.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A delivery adds its quantity to stock, and a reduction removes it, from the movement date onward; availability before that date is unchanged.

## Rationale

Plans can count on announced deliveries without overstating today's stock.

## Examples

- 10 extra lights delivered on 1 June are available for a project on 2 June but not on 31 May.

## Exceptions

None.
