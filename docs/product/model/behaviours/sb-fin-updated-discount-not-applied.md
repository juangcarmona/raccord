---
id: SB-FIN-UPDATED-DISCOUNT-NOT-APPLIED
type: structured-behaviour
title: "Raised customer discount does not reach a running project"
status: draft
illustrates:
  - "BR-FIN-DEFAULTS-NOT-RETROACTIVE"
given:
  - "A running project applied the customer's 10% rental discount"
when: "The customer's rental discount is raised to 15%"
then:
  - "The running project keeps 10%"
  - "Re-selecting the client on the project applies 15%"
uses-terms:
  - "TERM-CUSTOMER-FINANCIAL-DEFAULTS"
provenance:
  source: ".private/research/help-center/articles/360013886600.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Agreed prices are stable.

## Boundaries

None.
