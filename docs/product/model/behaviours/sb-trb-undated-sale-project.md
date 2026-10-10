---
id: "SB-TRB-UNDATED-SALE-PROJECT"
type: structured-behaviour
title: "Sale project without dates is found with the without-date selection"
status: draft
illustrates:
  - "BR-TRB-OVERVIEW-PERIOD-OVERLAP"
  - "BR-PRJ-SALE-PERIOD-OPTIONAL"
given:
  - "A cable sale project was saved without any time period"
when: "The coordinator selects a date period in the overview"
then:
  - "The cable sale project is not listed"
  - "Choosing the without-date selection lists it"
uses-terms:
  - "TERM-SALE-PROJECT"
provenance:
  source: ".private/research/help-center/articles/360014514680.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Undated projects have their own selection.

## Boundaries

None.
