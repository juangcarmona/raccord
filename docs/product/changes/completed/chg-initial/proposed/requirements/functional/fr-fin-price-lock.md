---
id: FR-FIN-PRICE-LOCK
type: functional-requirement
title: "Lock project prices"
status: draft
derived-from:
  - "UC-FIN-LOCK-PROJECT-PRICE"
  - "BR-FIN-PRICE-LOCK"
verification:
  - scenario-ref: "SB-FIN-LOCKED-PRICE-UNCHANGED"
uses-terms:
  - "TERM-PRICE-LOCK"
provenance:
  source: ".private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let users lock and unlock a project price so that plan changes do not alter locked prices.

## Rationale

Agreed prices must be stable.
