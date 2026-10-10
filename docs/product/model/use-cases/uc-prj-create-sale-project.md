---
id: UC-PRJ-CREATE-SALE-PROJECT
type: use-case
title: "Record an equipment sale project"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-WAREHOUSE-TECHNICIAN"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-SALE-PERIOD-OPTIONAL"
  - "BR-PRJ-SALE-STOCK-ON-BOOKING"
uses-terms:
  - "TERM-SALE-PROJECT"
provenance:
  source: ".private/research/help-center/articles/28922703497490.md, .private/research/help-center/articles/29017857097618.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Equipment is sold and stock is reduced when it leaves.

## Trigger

A customer buys equipment.

## Preconditions

A project type of the without-times sale category exists.

## Main Flow

1. The coordinator creates a project of the sale category.
2. The coordinator adds one period at the moment of the sale (recommended) and the sale items.
3. Items are booked out in the warehouse; the product reduces stock at booking.

## Alternative Flows

- No period added: stock is not adjusted automatically and must be corrected by hand.
- Sale items planned in a regular project: equally possible.

## Failure Conditions

- Creating a sale project when no project type of the sale category exists is refused with the reason.
- Without a time period, stock is not reduced automatically at booking and must be corrected by hand (BR-PRJ-SALE-PERIOD-OPTIONAL, BR-PRJ-SALE-STOCK-ON-BOOKING).
- Planning or reserving sale items does not change stock until they are booked out (BR-PRJ-SALE-STOCK-ON-BOOKING).
- An actor without permission to create projects is refused and nothing changes.

## Postconditions

Stock reflects the sale.
