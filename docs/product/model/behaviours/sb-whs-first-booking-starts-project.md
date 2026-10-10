---
id: SB-WHS-FIRST-BOOKING-STARTS-PROJECT
type: structured-behaviour
title: "First booking starts project progress"
status: draft
illustrates:
  - "BR-WHS-AUTOMATIC-PROGRESS"
given:
  - "A confirmed project has no bookings"
when: "The first item is booked"
then:
  - "The project status becomes in progress"
  - "The project's warehouse progress indicator leaves grey"
uses-terms:
  - "TERM-WAREHOUSE-BOOKING"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Automatic progress.

## Boundaries

None.
