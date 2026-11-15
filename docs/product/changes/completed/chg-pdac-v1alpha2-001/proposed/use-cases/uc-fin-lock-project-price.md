---
id: UC-FIN-LOCK-PROJECT-PRICE
type: use-case
title: "Lock or unlock the project price"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-PRICE-LOCK"
uses-terms:
  - "TERM-PRICE-LOCK"
provenance:
  source: ".private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The agreed project price stays fixed while the plan keeps evolving.

## Trigger

A quote or invoice has been generated and the price is agreed.

## Preconditions

The project has a financial overview.

## Main Flow

1. The coordinator locks the project price.
2. The product freezes prices, discounts and totals.
3. Later changes to planned equipment, crew and transport no longer change the financial overview.

## Alternative Flows

Unlock: the coordinator releases the lock and the product recalculates prices from the current plan.

## Failure Conditions

- Locking a project without a financial overview is refused; nothing changes.
- While locked, planning changes do not alter the agreed figures, so users do not see price changes (BR-FIN-PRICE-LOCK).
- An actor without permission to change project finances is refused and nothing changes.
- If another user changed the project in the meantime, the lock or unlock is refused and the coordinator must reload and retry.

## Postconditions

The project price is locked (or recalculated after unlocking).
