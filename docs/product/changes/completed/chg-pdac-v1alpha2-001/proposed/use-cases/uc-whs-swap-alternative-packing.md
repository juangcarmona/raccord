---
id: UC-WHS-SWAP-ALTERNATIVE-PACKING
type: use-case
title: "Swap in an alternative while packing"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors: []
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-WHS-PACKING-ALTERNATIVE-PRICE"
uses-terms:
  - "TERM-EQUIPMENT-ALTERNATIVE"
provenance:
  source: ".private/research/help-center/articles/13901986840082.md, .private/research/help-center/articles/14074151954834.md, .private/research/help-center/articles/14004547874834.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A planned article is replaced on the spot.

## Trigger

The planned unit is unavailable or unsuitable at packing time.

## Preconditions

A list is being booked.

## Main Flow

1. The technician selects a line and chooses another article.
2. The product replaces it at the planned price.

## Alternative Flows

- Inside a virtual combination the alternative gets price zero.

## Failure Conditions

- A replacement article that cannot be found or is unavailable is refused with the reason and the planned line stays.
- The replacement takes the planned price, or zero inside a virtual combination; a different price must be set in the project afterwards (BR-WHS-PACKING-ALTERNATIVE-PRICE).
- An actor without permission to book is refused and nothing changes.
- If another user changed the line in the meantime, the swap is refused and the technician must reload and retry.

## Postconditions

The line reflects what actually leaves.
