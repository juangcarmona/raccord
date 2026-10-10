---
id: FR-FIN-INSURANCE-DEPOSIT
type: functional-requirement
title: "Insurance and refundable deposit"
status: draft
derived-from:
  - "UC-FIN-CHARGE-INSURANCE"
  - "UC-FIN-MANAGE-REFUNDABLE-DEPOSIT"
  - "BR-FIN-INSURANCE-ON-GROSS-RENTAL"
  - "BR-FIN-DEPOSIT-RULES"
verification:
  - scenario-ref: "SB-FIN-INSURANCE-ON-GROSS"
  - scenario: "A 500.00 deposit is recorded as not paid, then paid, then returned."
uses-terms:
  - "TERM-INSURANCE-CHARGE"
  - "TERM-REFUNDABLE-DEPOSIT"
provenance:
  source: ".private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST support an insurance percentage on gross rental and a fixed refundable deposit with a tracked status.

## Rationale

Rental risk is covered commercially.
