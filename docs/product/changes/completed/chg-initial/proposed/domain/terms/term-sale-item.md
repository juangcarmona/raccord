---
id: TERM-SALE-ITEM
type: domain-term
title: "Sale item"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms:
  - "consumable"
  - "sales item"
uses-terms:
  - "TERM-EQUIPMENT-USAGE"
  - "TERM-SALE-PRICE"
  - "TERM-PURCHASE-PRICE"
  - "TERM-STOCK-MANAGEMENT-MODE"
  - "TERM-CRITICAL-STOCK"
provenance:
  source: ".private/research/help-center/articles/13021031495698.md, .private/research/help-center/articles/360013812939.md, .private/research/help-center/articles/360013580119.md"
  confidence: high
  recovered-from: documentation
---

## Definition

An equipment item with sale usage: sold to customers or consumed on jobs. It has a sale price and a purchase price, a stock management mode (tracked or excluded from tracking) and, when tracked, an optional critical stock level.

Its stock changes only through warehouse bookings (going out reduces stock, coming back restores it) and through manual stock movements; planning it on a project does not change stock. It may be default content of a physical kit, where it is never bound to the kit's stock.

## Distinguish From

- Rental item: returns after use.
- Temporary item: a one-off project line, not a catalog record.

## Usage

Consumables and merchandise; counted in inventory counts like rental items, with differences applied as stock movements.
