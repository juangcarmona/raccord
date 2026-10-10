---
id: UC-INT-LEAVE-EQUIPMENT-BEHIND
type: use-case
title: "Leave planned equipment behind when packing"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors: []
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-INT-LEAVE-BEHIND-REMOVES-LINE"
uses-terms:
  - "TERM-WAREHOUSE-STATUS"
provenance:
  source: ".private/research/help-center/articles/37629880908050.md"
  confidence: high
  recovered-from: interview
---

## Goal

Equipment that should not go out after all does not block the packing moment.

## Trigger

An item is damaged or over-ordered while packing.

## Preconditions

The technician has the leave-behind permission.

## Main Flow

1. The technician marks the item as left behind while booking.
2. The product removes the item from the project.
3. The packing moment's status follows the remaining items.

## Alternative Flows

None.

## Failure Conditions

Without the permission the action is not offered.

## Postconditions

The item is no longer in the project.
