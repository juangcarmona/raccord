---
id: BR-WHS-PRICE-FROM-USAGE
type: business-rule
title: "Line price follows its own usage period"
status: draft
applies-to:
  - "UC-WHS-SET-LINE-TIMES"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The price factor and total of each planned line are recalculated from that line's own usage period or, when a duration is used instead of a period, from that duration.

## Rationale

Equipment rented for part of a production is charged for that part.

## Examples

Projectors used Wednesday to Friday of a Monday-to-Friday project are charged three days.

## Exceptions

None.
