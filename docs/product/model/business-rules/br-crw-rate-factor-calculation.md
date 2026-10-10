---
id: BR-CRW-RATE-FACTOR-CALCULATION
type: business-rule
title: "Crew rates are calculated band by band"
status: draft
applies-to:
  - "UC-CRW-MANAGE-CREW-RATES"
  - "UC-CRW-RECONCILE-PLANNED-AND-REGISTERED-TIME"
uses-terms:
  - "TERM-RATE-FACTOR"
  - "TERM-OVERTIME"
provenance:
  source: ".private/research/help-center/articles/7191443180562.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

A crew rate's amount for a number of hours is the fixed amount of the band the hours fall into plus the hourly amounts of every band beyond the first fixed band for each hour worked within it, in band order.

## Rationale

Combines day rates with overtime steps.

## Examples

See SB-CRW-OVERTIME-ONE-HOUR and SB-CRW-OVERTIME-THREE-HOURS.

## Exceptions

Each assignment is calculated independently; project and period totals are the sum of the assignment totals, with no hours or rate bands carried between assignments.
