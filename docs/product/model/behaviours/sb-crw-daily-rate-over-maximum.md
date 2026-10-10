---
id: SB-CRW-DAILY-RATE-OVER-MAXIMUM
type: structured-behaviour
title: "Hours beyond the daily maximum"
status: draft
illustrates:
  - "BR-CRW-DAILY-RATE-MAXIMUM-HOURS"
given:
  - "A rate has daily rate 400, hourly rate 50 and a maximum of 10 hours per day"
when: "The crew member works 11 hours in one day"
then:
  - "The day costs 450: the daily rate plus one hour at the hourly rate"
uses-terms:
  - "TERM-CREW-RATE"
provenance:
  source: ".private/research/help-center/articles/360013323900.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intent

Simple day rate with overtime.

## Boundaries

None.
