---
id: BR-EQP-SELL-RENTAL-VIA-SALE-COPY
type: business-rule
title: "Selling rental equipment uses a sale copy"
status: draft
applies-to:
  - "UC-EQP-SELL-RENTAL-EQUIPMENT"
uses-terms:
  - "TERM-RENTAL-ITEM"
  - "TERM-SALE-ITEM"
provenance:
  source: ".private/research/help-center/articles/360013812939.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Rental equipment that is sold is handled by reducing the rental item's stock by the sold quantity and selling a separate sale item copy, never by changing the usage of the rental item itself.

## Rationale

The rental item's history and planning remain correct.

## Examples

- Selling 2 of 10 lights: rental stock becomes 8, and a sale item for 2 is booked to the customer.

## Exceptions

None.
