---
id: UC-EQP-VIEW-ITEM-INSIGHTS
type: use-case
title: "Review the state of an item's stock"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by: []
uses-terms:
  - "TERM-EQUIPMENT-ITEM"
  - "TERM-STOCK-QUANTITY"
  - "TERM-LOST-EQUIPMENT"
  - "TERM-DELAYED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/13141562920338.md"
  confidence: medium
  recovered-from: documentation
---

## Goal

The manager sees at a glance where an item's units are.

## Trigger

A question about an item's whereabouts or condition.

## Preconditions

- The item exists.

## Main Flow

1. The manager opens the item's overview.
2. The product shows total stock and availability and counts per state: delayed, booked on project, planned, reserved, lost.
3. The product lists projects currently holding units, grouped by status, with quantities per project.
4. The product lists the item's lost equipment.

## Alternative Flows

- 1a. The manager also opens the item's inventory counts.

## Failure Conditions

- The manager lacks permission to see the item or its stock: the overview is refused and nothing is shown.
- The item has no units in a given state, or no lost equipment: those parts are shown with zero or empty.
- The item no longer exists: the overview is refused with that reason.
- The product cannot be reached: the overview fails to load visibly and can be retried.

## Postconditions

- The manager knows the item's current distribution.
