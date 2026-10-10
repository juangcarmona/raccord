---
id: SB-WHS-LOSS-INVALIDATES-RESERVATIONS
type: structured-behaviour
title: "Losing units invalidates later reservations"
status: draft
illustrates:
  - "BR-WHS-INVALIDATION-ON-STOCK-CHANGE"
  - "UC-WHS-MARK-LOST"
given:
  - "6 units are owned"
  - "Projects X and Y next week reserve 3 each"
when: "2 units are marked lost on a returning job"
then:
  - "Stock becomes 4"
  - "X and Y show invalid reservation shortages totalling 2"
uses-terms:
  - "TERM-INVALID-RESERVATION"
provenance:
  source: ".private/research/help-center/articles/360013836939.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Reservations are not silently trimmed.

## Boundaries

Which of X or Y bears the shortage is not specified by the documentation.
