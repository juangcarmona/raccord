---
id: SB-FIN-LOCKED-PRICE-UNCHANGED
type: structured-behaviour
title: "Adding equipment to a locked project"
status: draft
illustrates:
  - "BR-FIN-PRICE-LOCK"
given:
  - "The project price is locked at 8,000.00"
when: "The coordinator adds two extra lights to the plan"
then:
  - "The project total stays 8,000.00"
  - "Unlocking recalculates the total including the lights"
uses-terms:
  - "TERM-PRICE-LOCK"
provenance:
  source: ".private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Locking protects agreed prices.

## Boundaries

None.
