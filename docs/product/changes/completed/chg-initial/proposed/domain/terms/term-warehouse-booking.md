---
id: TERM-WAREHOUSE-BOOKING
type: domain-term
title: "Warehouse booking"
status: draft
defined-in: BC-WAREHOUSE
synonyms:
  - "booking"
uses-terms:
  - "TERM-WAREHOUSE-STATUS"
  - "TERM-PACKING-LIST"
  - "TERM-CHECK-OUT"
  - "TERM-CHECK-IN"
provenance:
  source: ".private/research/help-center/articles/13901986722322.md, .private/research/help-center/articles/13901986840082.md, .private/research/help-center/articles/360015601560.md, .private/research/help-center/articles/14004547874834.md"
  confidence: high
  recovered-from: documentation
---

## Definition

Recording that a quantity (or a combination with its content) of a packing list moved from one warehouse status to another, by manual entry, code scanning or typed quantity. Each booking records who, when, what and between which statuses, and appears in the tracking log. Booking is optional for rental availability (which follows planning) but mandatory for sale stock to change.

## Distinguish From

Not a reservation: booking records physical movement.

## Usage

Core warehouse action; check-out and check-in are bookings in particular directions.
