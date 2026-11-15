---
id: UC-FIN-CHARGE-INSURANCE
type: use-case
title: "Charge insurance for rented equipment"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-INSURANCE-ON-GROSS-RENTAL"
uses-terms:
  - "TERM-INSURANCE-CHARGE"
provenance:
  source: ".private/research/help-center/articles/360013264720.md, .private/research/help-center/articles/360013878320.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The customer is charged for insuring the rented equipment.

## Trigger

The company asks insurance for this job.

## Preconditions

The project has rental equipment.

## Main Flow

1. The coordinator enters an insurance percentage.
2. The product computes the charge on the gross rental price and adds it to the project total, booked on the insurance ledger.
3. Quotes and invoices generated afterwards show the charge.

## Alternative Flows

Setting the percentage to zero removes the charge.

## Failure Conditions

- Charging insurance on a project without rental equipment is refused; nothing is added.
- A percentage that is negative or not a number is refused with the reason and the previous charge stays (BR-FIN-INSURANCE-ON-GROSS-RENTAL).
- An actor without permission to edit the project's finances is refused and nothing changes.
- If another user changed the project in the meantime, the change is refused and the coordinator must reload and retry.

## Postconditions

The project total includes the insurance charge.
