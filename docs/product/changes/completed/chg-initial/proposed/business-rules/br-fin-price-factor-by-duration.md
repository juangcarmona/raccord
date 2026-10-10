---
id: BR-FIN-PRICE-FACTOR-BY-DURATION
type: business-rule
title: "Price factor follows the usage period"
status: draft
applies-to:
  - "UC-FIN-CONFIGURE-PRICE-FACTORS"
  - "UC-FIN-REVIEW-PROJECT-FINANCIALS"
uses-terms:
  - "TERM-PRICE-FACTOR"
  - "TERM-FACTOR-GROUP"
provenance:
  source: ".private/research/help-center/articles/360013944139.md, .private/research/help-center/articles/360015935839.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The unit price of planned equipment is multiplied by the factor that the item's factor group assigns to the number of days of the equipment group's usage period; the factor can be overwritten on the project line.

## Rationale

Long rentals get a better price without manual discounting.

## Examples

See SB-FIN-PRICE-FACTOR-FOUR-DAYS.

## Exceptions

A manually overwritten factor is kept.
