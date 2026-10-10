---
id: TERM-COUNT-RESULT
type: domain-term
title: "Count result"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms:
  - "count status"
uses-terms:
  - "TERM-INVENTORY-COUNT"
  - "TERM-EXPECTED-WAREHOUSE-STOCK"
  - "TERM-LOST-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360015960120.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The outcome of one line of an inventory count, derived by comparing the counted quantity with the expected warehouse stock:

| Result | Meaning |
| --- | --- |
| Correct | Counted equals expected. |
| Shortage | Fewer counted than expected. |
| Surplus | More counted than expected. |

Lifecycle of the count itself: open (counting in progress) to completed. Completing applies the differences to stock.

## Distinguish From

- Shortage in planning: demand above availability; a count shortage means missing physical stock.

## Usage

Drives stock adjustments and lost-equipment records after a count.
