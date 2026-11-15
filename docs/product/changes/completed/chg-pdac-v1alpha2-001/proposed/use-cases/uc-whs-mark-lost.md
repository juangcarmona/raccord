---
id: UC-WHS-MARK-LOST
type: use-case
title: "Mark equipment lost"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors: []
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-WHS-DEFECT-LOSS-TIMING"
  - "BR-WHS-CHARGE-TO-PROJECT"
  - "BR-WHS-LOSS-REDUCES-STOCK"
  - "BR-WHS-INVALIDATION-ON-STOCK-CHANGE"
uses-terms:
  - "TERM-LOST-EQUIPMENT"
  - "TERM-DAMAGE-CHARGE"
provenance:
  source: ".private/research/help-center/articles/17531415206418.md, .private/research/help-center/articles/13901986840082.md, .private/research/help-center/articles/360014392200.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Missing equipment leaves stock and may be charged.

## Trigger

Equipment does not come back.

## Preconditions

Equipment is in a status before returned.

## Main Flow

1. The technician marks a line lost with quantity, charge decision and remark.
2. The product records the loss, reduces stock and adds any charge.

## Alternative Flows

- None.

## Failure Conditions

- Reservations elsewhere may become invalid.

## Postconditions

A lost record exists.
