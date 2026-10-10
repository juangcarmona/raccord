---
id: SB-FIN-ESTIMATED-CREW-COST
type: structured-behaviour
title: "Estimated cost of a crew function with fixed and hourly cost"
status: draft
illustrates:
  - "BR-FIN-ESTIMATED-FUNCTION-COST"
given:
  - "A crew function has a fixed cost of 50.00, an hourly cost of 25.00, a duration of 10 hours and a quantity of 2"
when: "The financial overview computes the estimated crew cost"
then:
  - "The function's estimated cost is 2 x (50 + 10 x 25) = 600.00"
  - "The estimated crew cost of the project includes 600.00 for this function"
uses-terms:
  - "TERM-ESTIMATED-COST"
provenance:
  source: ".private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Fixes the formula for function-based crew estimates.

## Boundaries

Planned and actual crew cost use other sources.
