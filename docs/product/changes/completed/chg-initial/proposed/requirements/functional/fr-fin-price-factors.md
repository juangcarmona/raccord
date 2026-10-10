---
id: FR-FIN-PRICE-FACTORS
type: functional-requirement
title: "Price rentals by duration factors"
status: draft
derived-from:
  - "UC-FIN-CONFIGURE-PRICE-FACTORS"
  - "BR-FIN-PRICE-FACTOR-BY-DURATION"
  - "BR-FIN-DEFAULT-FACTOR-GROUP"
verification:
  - scenario-ref: "SB-FIN-PRICE-FACTOR-FOUR-DAYS"
uses-terms:
  - "TERM-PRICE-FACTOR"
provenance:
  source: ".private/research/help-center/articles/360013944139.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST multiply equipment unit prices by the factor of the item's factor group for the usage period length, allowing a manual override per line.

## Rationale

Multi-day rentals are standard in the trade.
