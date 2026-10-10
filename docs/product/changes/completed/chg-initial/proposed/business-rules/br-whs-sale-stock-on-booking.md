---
id: BR-WHS-SALE-STOCK-ON-BOOKING
type: business-rule
title: "Sale stock changes only when booked"
status: draft
applies-to:
  - "UC-WHS-RETURN-SALE-ITEMS"
  - "UC-WHS-BOOK-MANUALLY"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-UNUSED-SALE-QUANTITY"
  - "TERM-SALES-SHORTAGE"
provenance:
  source: ".private/research/help-center/articles/14003895309330.md, .private/research/help-center/articles/13901986722322.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The stock of a sale article decreases when it is booked out and increases by the unused quantity booked back; planning a sale article does not change its stock.

## Rationale

How much is consumed is only known after the job.

## Examples

See SB-WHS-SALE-UNUSED-RETURN.

## Exceptions

None.
