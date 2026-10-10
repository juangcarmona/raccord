---
id: BR-PRJ-SALE-STOCK-ON-BOOKING
type: business-rule
title: "Sale stock changes on booking, given a period"
status: draft
applies-to:
  - "UC-PRJ-CREATE-SALE-PROJECT"
uses-terms:
  - "TERM-SALE-PROJECT"
provenance:
  source: ".private/research/help-center/articles/28922703497490.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Stock of sale items does not change when they are planned or reserved but when they are booked out (and back in if returned); automatic stock adjustment on booking requires the project to have a time period, otherwise stock must be adjusted manually.

## Rationale

Sales are final only when goods leave.

## Examples

See SB-PRJ-SALE-WITHOUT-PERIOD-NO-STOCK-CHANGE.

## Exceptions

None.
