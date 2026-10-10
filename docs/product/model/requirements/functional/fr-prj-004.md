---
id: FR-PRJ-004
type: functional-requirement
title: "Use planning period for availability and usage period for charging"
status: draft
derived-from:
  - "BR-PRJ-PLANNING-PERIOD-AVAILABILITY"
  - "BR-PRJ-USAGE-PERIOD-PRICING"
verification:
  - scenario: "A planned item is unavailable to other projects throughout the planning period"
  - scenario: "Rental is priced over the usage period's length"
uses-terms:
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360017700339.md, .private/research/help-center/articles/360013599680.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST calculate equipment and crew availability over planning periods and MUST base the charged period and price factor on usage periods.

## Rationale

Separates logistics from billing.
