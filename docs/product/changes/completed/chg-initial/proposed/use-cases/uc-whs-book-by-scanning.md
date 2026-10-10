---
id: UC-WHS-BOOK-BY-SCANNING
type: use-case
title: "Book equipment by scanning codes"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors: []
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-WHS-DEFAULT-BOOKING-TARGET"
  - "BR-WHS-UNPLANNED-SCAN-ADDS"
  - "BR-WHS-NO-EARLY-CHECK-OUT"
  - "BR-WHS-BOOK-OUT-TAKES-RESERVATION"
  - "BR-WHS-AUTOMATIC-PROGRESS"
  - "BR-WHS-BOOKING-MARKS-READY"
uses-terms:
  - "TERM-SCAN-CODE"
  - "TERM-WAREHOUSE-BOOKING"
provenance:
  source: ".private/research/help-center/articles/13901986840082.md, .private/research/help-center/articles/360015601560.md, .private/research/help-center/articles/360013478180.md, .private/research/help-center/articles/360015609959.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Each scan books the right article.

## Trigger

Packing or receiving with labelled equipment.

## Preconditions

Articles carry scan codes.

## Main Flow

1. The technician starts a list on a phone camera or a webcam.
2. Each scanned code books one unit (or the identified case) to the target status, with optional sound feedback.
3. The product updates the list in real time.

## Alternative Flows

- Unplanned code: product asks to add it as additional equipment.
- Scanning the card's code opens the list on a device.

## Failure Conditions

- Unknown code: not booked.

## Postconditions

Scanned articles are booked.
