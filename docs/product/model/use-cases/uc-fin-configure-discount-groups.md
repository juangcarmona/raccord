---
id: UC-FIN-CONFIGURE-DISCOUNT-GROUPS
type: use-case
title: "Configure discount groups"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-BILLING-ADMINISTRATOR"
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-DISCOUNT-GROUP-PERCENTAGE"
uses-terms:
  - "TERM-DISCOUNT-GROUP"
provenance:
  source: ".private/research/help-center/articles/360013890780.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Customers get different discounts per kind of equipment.

## Trigger

Customer agreements vary per equipment type.

## Preconditions

None.

## Main Flow

1. The administrator creates discount groups.
2. The administrator assigns equipment items to a group.
3. For each customer the billing administrator enters a percentage per group.

## Alternative Flows

None.

## Failure Conditions

- A group without a name, or a percentage outside zero to one hundred, is refused with the reason and nothing is saved.
- Equipment in no group, or a customer with no percentage for a group, receives no group discount (BR-FIN-DISCOUNT-GROUP-PERCENTAGE).
- An actor without permission to configure discounts is refused and nothing changes.
- If another user changed the same record in the meantime, the change is refused and the user must reload and retry.

## Postconditions

Projects for that customer receive group discounts.
