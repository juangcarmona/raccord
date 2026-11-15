---
id: UC-EQP-SCHEDULE-STOCK-MOVEMENT
type: use-case
title: "Schedule a delivery or reduction of stock"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-MOVEMENT-EFFECTIVE-DATE"
uses-terms:
  - "TERM-SCHEDULED-STOCK-MOVEMENT"
  - "TERM-STOCK-MOVEMENT"
  - "TERM-SALE-ITEM"
provenance:
  source: ".private/research/help-center/articles/16459216697106.md, .private/research/help-center/articles/360015957259.md, .private/research/help-center/articles/360013812939.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Expected purchases and planned disposals are reflected in future availability.

## Trigger

Equipment is ordered, or will be sold or disposed of.

## Preconditions

- The item is a rental item with its own stock, or a tracked sale item.

## Main Flow

1. The equipment manager opens the item's stock history and schedules a movement.
2. The manager enters a positive quantity (delivery) or negative quantity (reduction), a date, and optionally a description and details.
3. The product records the movement; availability changes from that date.

## Alternative Flows

None.

## Failure Conditions

- Virtual kit: not offered, because its stock is derived from its content.

## Postconditions

- Stock over time includes the movement.
