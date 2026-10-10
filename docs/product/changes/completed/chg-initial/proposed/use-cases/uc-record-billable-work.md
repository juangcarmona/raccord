---
id: UC-RECORD-BILLABLE-WORK
type: use-case
title: "Record billable work"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-LINE-PRICING-BOUNDS"
  - "BR-UNIT-PRICE-PRECISION"
  - "BR-MONEY-ROUNDING"
  - "BR-LINE-ORDER"
uses-terms:
  - "TERM-BILLABLE-LINE"
  - "TERM-BILLING-STATE"
---

## Goal

Work done or to be done on a job is ready to be billed.

## Trigger

Equipment, labour, transport, consumables or services are agreed or delivered.

## Preconditions

The project exists.

## Main Flow

1. The coordinator chooses the kind and enters description, quantity, unit price, discount and VAT rate.
2. The product calculates the net amount and adds the line as unbilled at the end of the project's lines.

## Alternative Flows

None.

## Failure Conditions

Values outside the pricing bounds are refused with the rule that was broken.

## Postconditions

The line is unbilled and visible on the project.
