---
id: QR-FIN-TAX-RATE-FRESHNESS
type: quality-requirement
title: "Location tax rates are at most one month stale"
status: draft
quality-attribute: "data currency"
applies-to:
  - "UC-FIN-MAINTAIN-LOCATION-TAX-RATES"
verification:
  - scenario-ref: "SB-FIN-MID-MONTH-RATE-CHANGE"
uses-terms:
  - "TERM-LOCATION-TAX-RATE"
provenance:
  source: ".private/research/help-center/articles/21200846347154.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

A location tax rate MUST reflect any official change no later than the first day of the following month and keep the full history.

## Measurement

For changed rates, compare the effective date with the update date: the delay never exceeds one calendar month; history contains every earlier rate.
