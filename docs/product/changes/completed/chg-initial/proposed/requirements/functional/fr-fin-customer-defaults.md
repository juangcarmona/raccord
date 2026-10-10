---
id: FR-FIN-CUSTOMER-DEFAULTS
type: functional-requirement
title: "Propose customer financial defaults"
status: draft
derived-from:
  - "UC-FIN-SET-CUSTOMER-FINANCIAL-DEFAULTS"
  - "UC-FIN-APPLY-CUSTOMER-DEFAULTS"
  - "BR-FIN-CUSTOMER-DEFAULTS-ON-SELECTION"
  - "BR-FIN-DEFAULTS-NOT-RETROACTIVE"
verification:
  - scenario-ref: "SB-FIN-CUSTOMER-DISCOUNTS-PROMPTED"
  - scenario-ref: "SB-FIN-UPDATED-DISCOUNT-NOT-APPLIED"
uses-terms:
  - "TERM-CUSTOMER-FINANCIAL-DEFAULTS"
provenance:
  source: ".private/research/help-center/articles/360013886600.md, .private/research/help-center/articles/360013628279.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST store discounts, payment term, invoice moment and taxation per customer and propose them, with confirmation, when the customer is selected as client, without changing running projects automatically.

## Rationale

Agreed conditions are applied consistently but never silently.
