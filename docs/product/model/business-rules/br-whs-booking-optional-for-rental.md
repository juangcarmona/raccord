---
id: BR-WHS-BOOKING-OPTIONAL-FOR-RENTAL
type: business-rule
title: "Rental availability follows planning, not booking"
status: draft
applies-to:
  - "UC-WHS-BOOK-MANUALLY"
  - "UC-WHS-REVIEW-WAREHOUSE-BOARD"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-WAREHOUSE-BOOKING"
  - "TERM-EQUIPMENT-AVAILABILITY"
provenance:
  source: ".private/research/help-center/articles/13901986722322.md, .private/research/help-center/articles/360014382840.md, .private/research/help-center/articles/360014458519.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Booking rental equipment in the warehouse is optional; rental availability is computed from planning and is only adjusted by bookings through early returns, delays and booking out reserved stock.

## Rationale

Smaller companies can plan without scanning.

## Examples

A company that never scans still gets correct shortages from its plans.

## Exceptions

None.
