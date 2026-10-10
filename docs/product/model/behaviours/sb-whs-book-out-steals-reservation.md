---
id: SB-WHS-BOOK-OUT-STEALS-RESERVATION
type: structured-behaviour
title: "Booking out a unit reserved elsewhere"
status: draft
illustrates:
  - "BR-WHS-BOOK-OUT-TAKES-RESERVATION"
  - "UC-WHS-BOOK-BY-SCANNING"
given:
  - "A microphone is reserved for project A"
  - "Project B in the same period plans it unreserved"
when: "The technician books the microphone out for B"
then:
  - "The microphone is reserved for B"
  - "A shows an invalid reservation shortage"
uses-terms:
  - "TERM-RESERVATION"
  - "TERM-INVALID-RESERVATION"
provenance:
  source: ".private/research/help-center/articles/360012699859.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Physical moves win.

## Boundaries

None.
