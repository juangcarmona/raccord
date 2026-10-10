---
id: BR-FIN-DEFAULTS-NOT-RETROACTIVE
type: business-rule
title: "Changed customer defaults do not reach running projects automatically"
status: draft
applies-to:
  - "UC-FIN-SET-CUSTOMER-FINANCIAL-DEFAULTS"
  - "UC-FIN-APPLY-CUSTOMER-DEFAULTS"
uses-terms:
  - "TERM-CUSTOMER-FINANCIAL-DEFAULTS"
provenance:
  source: ".private/research/help-center/articles/360013886600.md, .private/research/help-center/articles/360013890780.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Changing a customer's discount percentages does not change projects where it is already client; category and total discounts are refreshed only by selecting the client again and discount-group percentages only by explicitly updating the equipment data of the project.

## Rationale

Agreed project prices stay stable.

## Examples

See SB-FIN-UPDATED-DISCOUNT-NOT-APPLIED.

## Exceptions

None.
