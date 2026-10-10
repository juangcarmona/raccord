---
id: SB-FIN-TYPED-TOTAL-BECOMES-DISCOUNT
type: structured-behaviour
title: "Typing a round total sets the discount"
status: draft
illustrates:
  - "BR-FIN-TOTAL-DISCOUNT"
given:
  - "The project total before project discount is 10,345.00"
when: "The coordinator types 10,000.00 as project total"
then:
  - "The project discount becomes 345.00"
  - "The project total is 10,000.00"
uses-terms:
  - "TERM-TOTAL-DISCOUNT"
provenance:
  source: ".private/research/help-center/articles/360013264720.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Negotiated round prices.

## Boundaries

None.
