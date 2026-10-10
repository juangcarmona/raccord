---
id: SB-WHS-SALE-UNUSED-RETURN
type: structured-behaviour
title: "Only unused consumables return to stock"
status: draft
illustrates:
  - "BR-WHS-SALE-STOCK-ON-BOOKING"
  - "UC-WHS-RETURN-SALE-ITEMS"
given:
  - "50 fireworks of a sale article were booked out"
  - "Sale stock was 200 before booking out"
when: "The technician books 15 back as unused"
then:
  - "Sale stock is 165"
uses-terms:
  - "TERM-UNUSED-SALE-QUANTITY"
provenance:
  source: ".private/research/help-center/articles/14003895309330.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Consumption is measured at return.

## Boundaries

None.
