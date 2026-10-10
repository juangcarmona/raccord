---
id: TERM-STOCK-MANAGEMENT-MODE
type: domain-term
title: "Stock management mode"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms: []
uses-terms:
  - "TERM-SALE-ITEM"
  - "TERM-CRITICAL-STOCK"
provenance:
  source: ".private/research/help-center/articles/13021031495698.md, .private/research/help-center/articles/360013580119.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A setting on sale items:

| Mode | Meaning |
| --- | --- |
| Track stock | Stock and availability are maintained; a critical stock level may be set. |
| Exclude from stock tracking | No stock or availability is kept; suited to cheap consumables that are always in supply. |

## Distinguish From

- Stock quantity: the number owned, which exists only when stock is tracked.

## Usage

Chosen when creating a sale item.
