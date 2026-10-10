---
id: SB-FIN-USAGE-PERCENTAGE
type: structured-behaviour
title: "Usage percentage of an item for one day"
status: draft
illustrates:
  - "BR-FIN-USAGE-PERCENTAGE"
given:
  - "20 units of an item exist"
  - "Bookings in the period add up to 5 days and 18 hours of unit time"
when: "Equipment statistics are viewed for 1 October"
then:
  - "Planned hours are 5 x 24 + 18 = 138"
  - "Usage is (138 / 20) / 24 x 100 = 28.75%"
uses-terms:
  - "TERM-USAGE-PERCENTAGE"
provenance:
  source: ".private/research/help-center/articles/18431827595666.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Fixes the utilisation formula.

## Boundaries

None.
