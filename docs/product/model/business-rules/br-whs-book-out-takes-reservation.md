---
id: BR-WHS-BOOK-OUT-TAKES-RESERVATION
type: business-rule
title: "Booking out moves the reservation to the booking project"
status: draft
applies-to:
  - "UC-WHS-BOOK-MANUALLY"
  - "UC-WHS-BOOK-BY-SCANNING"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-RESERVATION"
  - "TERM-INVALID-RESERVATION"
  - "TERM-CHECK-OUT"
provenance:
  source: ".private/research/help-center/articles/360012699859.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Booking out units that were reserved for another project reserves them for the project being booked; the other project's reservation may become invalid and shows as a shortage there.

## Rationale

Physical reality overrides planned priority.

## Examples

See SB-WHS-BOOK-OUT-STEALS-RESERVATION.

## Exceptions

None.
