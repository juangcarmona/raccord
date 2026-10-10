---
id: UC-WHS-MARK-FOUND
type: use-case
title: "Mark lost equipment as found"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors: []
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-WHS-LOSS-REDUCES-STOCK"
uses-terms:
  - "TERM-LOST-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360014392200.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Recovered units return to stock.

## Trigger

Lost equipment turns up.

## Preconditions

A lost record exists.

## Main Flow

1. The technician selects the record, marks it found with the find date and confirms.
2. The product counts the units back.

## Alternative Flows

- None.

## Failure Conditions

- Marking found without a find date, or with a date in the future, is refused with the reason and stock is unchanged (BR-WHS-LOSS-REDUCES-STOCK).
- Marking a record that is already found is refused.
- Without confirmation, nothing is counted back.
- An actor without permission to manage stock is refused and nothing changes.
- If another user changed the record in the meantime, the change is refused and the technician must reload and retry.

## Postconditions

Units available again.
