---
id: UC-EQP-SELL-RENTAL-EQUIPMENT
type: use-case
title: "Sell equipment that was rented out"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-SELL-RENTAL-VIA-SALE-COPY"
uses-terms:
  - "TERM-RENTAL-ITEM"
  - "TERM-SALE-ITEM"
  - "TERM-EQUIPMENT-RECORD-STATE"
provenance:
  source: ".private/research/help-center/articles/360013812939.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Rental units sold to a customer leave rental stock and are invoiced as a sale.

## Trigger

A customer buys used rental equipment.

## Preconditions

- The rental item exists with enough stock.

## Main Flow

1. The equipment manager duplicates the rental item and sets the copy to sale.
2. The manager reduces the rental item's stock by the quantity sold.
3. The sale copy is planned and booked on the customer's project, bringing its stock to zero.
4. If no units remain, the manager archives the rental item.

## Alternative Flows

- 2a. The manager adds a note explaining the stock change.

## Failure Conditions

- The manager lacks permission to change items or stock: the sale preparation is refused and nothing changes.
- The quantity sold exceeds the rental item's stock: the stock reduction is refused with the reason and nothing is recorded.
- Changing the rental item itself to sale is refused; the sale is handled only through a separate copy (BR-EQP-SELL-RENTAL-VIA-SALE-COPY).
- Another user changed the same item's stock in the meantime: the change is refused and the manager must reload and retry.

## Postconditions

- Rental stock and sales reflect the transaction.
