---
id: SB-WHS-NO-EARLY-BOOKING
type: structured-behaviour
title: "Booking before the planning period is refused"
status: draft
illustrates:
  - "BR-WHS-NO-EARLY-CHECK-OUT"
  - "UC-WHS-BOOK-MANUALLY"
given:
  - "A list's planning period starts tomorrow"
when: "The technician tries to book it out today"
then:
  - "The booking is refused"
uses-terms:
  - "TERM-PLANNING-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360012699859.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Planning period bounds bookings.

## Boundaries

None.
