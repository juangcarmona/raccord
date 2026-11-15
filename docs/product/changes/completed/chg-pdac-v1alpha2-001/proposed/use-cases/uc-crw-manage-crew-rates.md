---
id: UC-CRW-MANAGE-CREW-RATES
type: use-case
title: "Define global crew rates"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-CREW-PLANNER"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-PRICE-RATES-ON-FUNCTIONS-ONLY"
  - "BR-CRW-RATE-CHANGE-NOT-RETROACTIVE"
  - "BR-CRW-RATE-FACTOR-CALCULATION"
uses-terms:
  - "TERM-CREW-RATE"
  - "TERM-RATE-FACTOR"
  - "TERM-RATE-KIND"
provenance:
  source: ".private/research/help-center/articles/7191443180562.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

The company has price and cost rates with hour bands.

## Trigger

Tariffs are introduced or changed.

## Preconditions

The user may manage financial settings.

## Main Flow

1. The user adds a rate and chooses price or cost.
2. The user names it and adds rate factors: hour thresholds with fixed and/or hourly amounts.
3. The product saves the rate.

## Alternative Flows

- An hourly amount is entered as a percentage of a previous band.
- Rates are sorted by name.

## Failure Conditions

- The user lacks the right to manage financial settings: the rate is refused and nothing is saved.
- The rate has no name, or its hour thresholds or amounts are missing or invalid: it is refused with the reason and nothing is saved (BR-CRW-RATE-FACTOR-CALCULATION).
- A price rate is linked to a crew member: it is refused, since price rates apply to functions only (BR-CRW-PRICE-RATES-ON-FUNCTIONS-ONLY).
- Another administrator changed the same rate in the meantime: the change is refused and the user must reload and retry.

## Postconditions

The rate can be chosen on functions (price and cost) or linked to crew (cost).
