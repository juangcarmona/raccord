---
id: SB-FIN-PRICE-FACTOR-FOUR-DAYS
type: structured-behaviour
title: "Four-day rental uses the four-day factor"
status: draft
illustrates:
  - "BR-FIN-PRICE-FACTOR-BY-DURATION"
given:
  - "The factor group assigns 2.40 to four days"
when: "The equipment group runs from 21 December 14:00 to 24 December 14:00 counted as four days"
then:
  - "Each item's unit price is multiplied by 2.40"
uses-terms:
  - "TERM-PRICE-FACTOR"
provenance:
  source: ".private/research/help-center/articles/360013944139.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Duration pricing is automatic.

## Boundaries

How partial days are counted is not asserted beyond this example.
