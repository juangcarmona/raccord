---
id: FR-FIN-101
type: functional-requirement
title: "Offer customers per-group discounts"
status: active
derived-from:
  - "UC-FIN-CONFIGURE-DISCOUNT-GROUPS"
  - "BR-FIN-DISCOUNT-GROUP-PERCENTAGE"
verification:
  - id: "DISCOUNT-GROUP-APPLIED"
    scenario: "Given a discount group of lighting with 10 percent for a customer, and a project for that customer containing a lighting item and an audio item, the lighting item is discounted by 10 percent and the audio item is not."
uses-terms:
  - "TERM-DISCOUNT-GROUP"
provenance:
  source: ".private/research/help-center/articles/360013890780.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let an administrator create discount groups and assign equipment items to them, and MUST let a billing administrator enter for each customer a percentage per group. The product MUST apply those percentages to the equipment of that customer's projects according to each item's group.

## Rationale

Customer agreements often differ by kind of equipment, and the discount must follow the agreement without manual adjustment per project.
