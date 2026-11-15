---
id: UC-WHS-RETURN-SALE-ITEMS
type: use-case
title: "Book back unused sale items"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors: []
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-WHS-SALE-STOCK-ON-BOOKING"
uses-terms:
  - "TERM-UNUSED-SALE-QUANTITY"
  - "TERM-CHECK-IN"
provenance:
  source: ".private/research/help-center/articles/14003895309330.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Sale stock reflects only what was consumed.

## Trigger

A job with consumables returns.

## Preconditions

Sale articles were booked out.

## Main Flow

1. When booking from on location to returned, the technician enters the unused quantity of each sale article.
2. After all equipment is back, the technician changes the status and closes the list.

## Alternative Flows

- None.

## Failure Conditions

- An unused quantity that is negative, not a number, or above the quantity booked out is refused with the reason and stock is unchanged (BR-WHS-SALE-STOCK-ON-BOOKING).
- Closing the list before all equipment is back is refused with the reason.
- An actor without permission to book is refused and nothing changes.
- If another user changed the list in the meantime, the booking is refused and the technician must reload and retry.

## Postconditions

Sale stock increased by the unused quantity.
