---
id: SB-WHS-BOOKING-UNMARKED-SUBPROJECT
type: structured-behaviour
title: "Booking an unmarked project"
status: draft
illustrates:
  - "BR-WHS-BOOKING-MARKS-READY"
  - "UC-WHS-BOOK-MANUALLY"
given:
  - "A warehouse manager sees a project not marked ready to pack"
when: "The manager books one item of it"
then:
  - "The product warns first"
  - "After continuing, the item is booked and the project is marked ready to pack"
uses-terms:
  - "TERM-READY-TO-PACK"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Reality updates the flag.

## Boundaries

None.
