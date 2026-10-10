---
id: FR-EQP-015
type: functional-requirement
title: "Count selected equipment"
status: draft
derived-from:
  - "UC-EQP-COUNT-SELECTED-EQUIPMENT"
  - "BR-EQP-EXPECTED-STOCK"
  - "BR-EQP-COUNT-COMPLETION"
  - "BR-EQP-COUNT-EXCLUDES-VIRTUAL-KITS"
verification:
  - scenario-ref: "SB-EQP-EXPECTED-STOCK"
  - scenario-ref: "SB-EQP-COUNT-SHORTAGE-BULK"
uses-terms:
  - "TERM-INVENTORY-COUNT"
  - "TERM-EXPECTED-WAREHOUSE-STOCK"
  - "TERM-COUNT-RESULT"
provenance:
  source: ".private/research/help-center/articles/360015960120.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let users choose the items to count, or all items, record the quantities found, show per line whether the count is correct, short or in surplus against the expected warehouse stock, and on completion correct stock with the differences and record shortfalls as lost equipment.

## Rationale

Periodic verification of stock.
