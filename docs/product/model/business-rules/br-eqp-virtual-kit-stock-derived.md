---
id: BR-EQP-VIRTUAL-KIT-STOCK-DERIVED
type: business-rule
title: "Virtual kit stock and availability come from content"
status: draft
applies-to:
  - "UC-EQP-DEFINE-VIRTUAL-KIT"
uses-terms:
  - "TERM-VIRTUAL-KIT"
  - "TERM-STOCK-QUANTITY"
  - "TERM-EQUIPMENT-AVAILABILITY"
provenance:
  source: ".private/research/help-center/articles/4402870209682.md, .private/research/help-center/articles/360013703140.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The number of virtual kits in stock (or available) is the smallest, over its rental content lines, of the content item's stock (or unreserved available quantity) divided by the quantity per kit, rounded down; sale items in the content are ignored for this figure, and planning the kit takes its content out of availability.

## Rationale

A virtual kit exists only through its parts.

## Examples

- Needs 1 A (stock 10), 3 B (stock 12), 4 C (stock 100): 4 kits in stock, limited by B.
- Available A 2, B 12, C 100: 2 kits available, limited by A.

## Exceptions

The availability timeline still shows the sale item's own calculation.
