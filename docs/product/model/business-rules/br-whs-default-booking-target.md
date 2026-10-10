---
id: BR-WHS-DEFAULT-BOOKING-TARGET
type: business-rule
title: "Default booking targets"
status: draft
applies-to:
  - "UC-WHS-BOOK-MANUALLY"
  - "UC-WHS-BOOK-BY-SCANNING"
uses-terms:
  - "TERM-CHECK-OUT"
  - "TERM-CHECK-IN"
provenance:
  source: ".private/research/help-center/articles/360015601560.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When booking out, the default target status is on location; when booking back, returned; the technician may choose any other status, including going back a step to undo a mistake.

## Rationale

Most bookings go straight out or straight in.

## Examples

A technician packing the day before chooses prepped instead.

## Exceptions

None.
