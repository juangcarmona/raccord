---
id: SB-WORKDAY-FORGOTTEN-CLOCK-OUT
type: structured-behaviour
title: "A forgotten clock-out leaves the day open"
status: draft
illustrates:
  - "BR-WORKDAY-OPEN-DAY"
given:
  - "An employee clocked in at 08:00 and never clocked out"
when: "The calendar day ends"
then:
  - "The day becomes Open with no end time"
  - "The employee and their manager are notified"
  - "No worked hours are counted until the day is corrected"
uses-terms:
  - "TERM-WORKDAY-RECORD"
---

## Intent

Missing data is surfaced, never guessed.

## Boundaries

None.
