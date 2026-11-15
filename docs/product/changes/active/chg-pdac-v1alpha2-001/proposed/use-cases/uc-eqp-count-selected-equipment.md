---
id: UC-EQP-COUNT-SELECTED-EQUIPMENT
type: use-case
title: "Count equipment against expected stock"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors:
  - "ACT-EQUIPMENT-MANAGER"
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-EXPECTED-STOCK"
  - "BR-EQP-COUNT-COMPLETION"
  - "BR-EQP-COUNT-EXCLUDES-VIRTUAL-KITS"
uses-terms:
  - "TERM-INVENTORY-COUNT"
  - "TERM-EXPECTED-WAREHOUSE-STOCK"
  - "TERM-COUNT-RESULT"
  - "TERM-LOST-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360015960120.md, .private/research/help-center/articles/360015957259.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The company verifies and fixes the stock of chosen items, or of all items, with a count.

## Trigger

A periodic or ad hoc stock count.

## Preconditions

- The items to count are single items, physical kits or sale items.

## Main Flow

1. The user selects items in the catalog, or all items, and starts a count, giving it a name and a remark; tasks and notes may be added.
2. The product lists the items with their expected warehouse stock at the moment of creation.
3. The user records the quantities found, entering them or scanning item codes with a phone camera or a webcam.
4. The product shows per line whether the count is correct, short or in surplus, so the user can review the differences.
5. The user completes the count.
6. The product corrects stock with the differences and records missing units as lost equipment.

## Alternative Flows

None.

## Failure Conditions

- None.

## Postconditions

- Stock of the counted items equals the count.
