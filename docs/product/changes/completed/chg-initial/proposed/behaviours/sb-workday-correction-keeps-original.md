---
id: SB-WORKDAY-CORRECTION-KEEPS-ORIGINAL
type: structured-behaviour
title: "A correction keeps the original clock-in"
status: draft
illustrates:
  - "BR-WORKDAY-APPEND-ONLY"
given:
  - "An employee clocked in at 09:47 but started at 09:00"
when: "A manager approves the correction to 09:00 with the reason 'forgot to clock in at arrival'"
then:
  - "The day shows 09:00 as the start"
  - "The original 09:47 entry remains visible with the correction, reason and approver"
uses-terms:
  - "TERM-WORKDAY-RECORD"
---

## Intent

Corrections are transparent.

## Boundaries

None.
