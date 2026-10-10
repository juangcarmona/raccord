---
id: FR-FIN-DISCOUNTS
type: functional-requirement
title: "Support discounts at every level"
status: draft
derived-from:
  - "UC-FIN-APPLY-PROJECT-DISCOUNTS"
  - "BR-FIN-CATEGORY-DISCOUNT"
  - "BR-FIN-TOTAL-DISCOUNT"
  - "BR-FIN-DISCOUNT-GROUP-PERCENTAGE"
  - "BR-FIN-DISCOUNT-ORDER"
verification:
  - scenario-ref: "SB-FIN-TYPED-TOTAL-BECOMES-DISCOUNT"
  - scenario-ref: "SB-FIN-DISCOUNT-GROUP-PER-CUSTOMER"
uses-terms:
  - "TERM-DISCOUNT-LEVEL"
provenance:
  source: ".private/research/help-center/articles/360013264720.md, .private/research/help-center/articles/360013886600.md, .private/research/help-center/articles/360013890780.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST support line, discount-group, category and project discounts (percentage or fixed amount, or a typed total) and apply them in a defined order.

## Rationale

Negotiation happens at different granularities.
