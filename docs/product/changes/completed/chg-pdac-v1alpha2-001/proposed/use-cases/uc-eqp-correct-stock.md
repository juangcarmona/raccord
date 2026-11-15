---
id: UC-EQP-CORRECT-STOCK
type: use-case
title: "Correct the stock of an item"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-CORRECTION-BULK-ONLY"
uses-terms:
  - "TERM-STOCK-CORRECTION"
  - "TERM-STOCK-QUANTITY"
provenance:
  source: ".private/research/help-center/articles/360015957259.md, .private/research/help-center/articles/360013812939.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Recorded stock matches reality.

## Trigger

Recorded stock is found to be wrong (for example an incomplete delivery).

## Preconditions

- The item has its own stock (it is not a virtual kit), or is a tracked sale item.

## Main Flow

1. The equipment manager opens the item's stock and starts a correction.
2. The manager enters the new stock level and optionally a name and description.
3. The manager confirms; the product records the difference as a movement.

## Alternative Flows

None.

## Failure Conditions

- The manager lacks permission to correct stock: the correction is refused and the recorded stock is unchanged.
- The item is a virtual kit: a stated new level is refused, because its stock is derived from its content (BR-EQP-CORRECTION-BULK-ONLY).
- The new stock level is missing or invalid, for example negative: it is refused with the reason and nothing is recorded.
- A past movement is not edited; the correction is recorded as a new movement instead.
- Another user changed the item's stock in the meantime: the correction is refused and the manager must reload and retry.

## Postconditions

- Stock equals the stated level.
