---
id: UC-FIN-MANAGE-ADDITIONAL-COSTS
type: use-case
title: "Manage the additional costs of a project"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-ADDITIONAL-COST-GROUPED"
  - "BR-FIN-ADDITIONAL-COST-ORDER"
  - "BR-FIN-ADDITIONAL-COST-TOTAL"
  - "BR-FIN-LOSS-CHARGED-AS-COST"
  - "BR-FIN-FINANCIAL-VISIBILITY"
uses-terms:
  - "TERM-ADDITIONAL-COST"
  - "TERM-COST-GROUP"
  - "TERM-DEFAULT-ADDITIONAL-COST"
provenance:
  source: ".private/research/help-center/articles/360013881760.md, .private/research/help-center/articles/4409044768658.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Non-equipment expenses are priced, grouped and passed on to the customer.

## Trigger

A job causes expenses such as hotels or parking.

## Preconditions

The project exists; the user has financial visibility.

## Main Flow

1. The coordinator adds a cost from scratch or from a default additional cost.
2. The coordinator enters name, purchase price, sale price, quantity, discount, VAT class, ledger and details and chooses its cost group.
3. The product computes the total and group subtotal.
4. The coordinator orders costs and groups; documents follow that order.

## Alternative Flows

Default costs are created or edited for reuse. Returned equipment marked lost or defective adds a cost automatically.

## Failure Conditions

User without financial visibility: adding costs is refused.

## Postconditions

The costs are part of the project price and estimated cost.
