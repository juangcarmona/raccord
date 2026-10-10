---
id: FR-WHS-032
type: functional-requirement
title: "Sale returns"
status: draft
derived-from:
  - "UC-WHS-RETURN-SALE-ITEMS"
  - "BR-WHS-SALE-STOCK-ON-BOOKING"
verification:
  - scenario-ref: "SB-WHS-SALE-UNUSED-RETURN"
uses-terms:
  - "TERM-UNUSED-SALE-QUANTITY"
provenance:
  source: ".private/research/help-center/articles/14003895309330.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST change sale stock only on booking, accepting the unused quantity at return.

## Rationale

Consumable accounting.
