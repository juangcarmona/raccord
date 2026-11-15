---
id: UC-FIN-APPLY-PROJECT-DISCOUNTS
type: use-case
title: "Apply discounts to a project"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-CATEGORY-DISCOUNT"
  - "BR-FIN-TOTAL-DISCOUNT"
  - "BR-FIN-DISCOUNT-ORDER"
  - "BR-FIN-PRICE-LOCK"
uses-terms:
  - "TERM-DISCOUNT-LEVEL"
  - "TERM-CATEGORY-DISCOUNT"
  - "TERM-TOTAL-DISCOUNT"
provenance:
  source: ".private/research/help-center/articles/360013264720.md, .private/research/help-center/articles/360013886600.md, .private/research/help-center/articles/360013881760.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The project price reflects the negotiated reductions before a quote or invoice is generated.

## Trigger

The customer negotiates the price.

## Preconditions

The project has priced content and is not price-locked.

## Main Flow

1. The coordinator sets a percentage discount on one or more revenue categories.
2. The coordinator sets a project discount as a percentage or a fixed amount, or types the desired project total.
3. The product recalculates category totals, the project total, profit and margin.

## Alternative Flows

Line discounts are set directly on equipment lines or additional costs; client defaults pre-fill discounts when applied.

## Failure Conditions

The project price is locked: discounts do not change totals until unlocked.

## Postconditions

The new discounts and totals are used by documents generated afterwards.
