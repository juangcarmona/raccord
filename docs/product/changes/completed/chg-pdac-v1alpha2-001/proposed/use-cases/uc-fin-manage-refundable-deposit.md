---
id: UC-FIN-MANAGE-REFUNDABLE-DEPOSIT
type: use-case
title: "Request and track a refundable deposit"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-CUSTOMER"
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-DEPOSIT-RULES"
uses-terms:
  - "TERM-REFUNDABLE-DEPOSIT"
  - "TERM-DEPOSIT-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The security deposit of a rental is requested and its payment and return are tracked.

## Trigger

The company wants security for valuable equipment.

## Preconditions

The project exists.

## Main Flow

1. The coordinator enters a fixed deposit amount.
2. The product sets the deposit status to not paid.
3. When the customer pays, the coordinator sets the status to paid.
4. When the deposit is given back, the coordinator sets it to returned and documents the refund with a credit note.

## Alternative Flows

The amount and status appear on documents only when the template includes them.

## Failure Conditions

- A deposit amount that is not a fixed positive number is refused with the reason and nothing is saved (BR-FIN-DEPOSIT-RULES).
- Marking a deposit returned without documenting the refund with a credit note is flagged as incomplete (BR-FIN-DEPOSIT-RULES).
- An actor without permission to change project finances is refused and nothing changes.
- If another user changed the deposit in the meantime, the change is refused and the coordinator must reload and retry.

## Postconditions

The deposit and its status are recorded on the project.
