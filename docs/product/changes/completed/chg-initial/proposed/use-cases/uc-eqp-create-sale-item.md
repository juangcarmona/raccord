---
id: UC-EQP-CREATE-SALE-ITEM
type: use-case
title: "Create a sale item"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-UNTRACKED-SALE-NO-AVAILABILITY"
  - "BR-WHS-SALE-STOCK-ON-BOOKING"
uses-terms:
  - "TERM-SALE-ITEM"
  - "TERM-SALE-PRICE"
  - "TERM-PURCHASE-PRICE"
  - "TERM-STOCK-MANAGEMENT-MODE"
  - "TERM-CRITICAL-STOCK"
provenance:
  source: ".private/research/help-center/articles/13021031495698.md, .private/research/help-center/articles/360013580119.md, .private/research/help-center/articles/360013812939.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A consumable or merchandise item is available for planning and, if desired, its stock and replenishment level are watched.

## Trigger

The company starts selling or consuming a product on jobs.

## Preconditions

- The equipment manager may maintain the catalog.

## Main Flow

1. The equipment manager creates an item and marks it as sale.
2. The manager enters the sale price and purchase price.
3. The manager chooses whether stock is tracked or excluded from tracking.
4. If tracked, the manager enters the quantity and optionally a critical stock level.
5. The product saves the item.

## Alternative Flows

- 3a. Excluded from tracking: no quantity is asked and the item never shows availability or shortages.
- 4a. Later, the manager filters the catalog for sale items at or below their critical stock to reorder them.

## Failure Conditions

- Name or sale price missing: the item is not saved.

## Postconditions

- The sale item can be planned; its stock changes only through warehouse bookings and manual stock movements.
