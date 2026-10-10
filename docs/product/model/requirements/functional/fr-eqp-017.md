---
id: FR-EQP-017
type: functional-requirement
title: "Track sale stock and critical levels"
status: draft
derived-from:
  - "UC-EQP-CREATE-SALE-ITEM"
  - "BR-EQP-UNTRACKED-SALE-NO-AVAILABILITY"
  - "BR-EQP-CRITICAL-STOCK-SIGNAL"
verification:
  - scenario: "A tracked sale item with critical stock 5 and stock 4 appears when filtering on critical stock."
uses-terms:
  - "TERM-SALE-ITEM"
  - "TERM-STOCK-MANAGEMENT-MODE"
  - "TERM-CRITICAL-STOCK"
provenance:
  source: ".private/research/help-center/articles/13021031495698.md, .private/research/help-center/articles/360013812939.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let users track or exclude stock for each sale item, set a critical stock level for tracked sale items, and filter items that reached it.

## Rationale

Consumable replenishment.
