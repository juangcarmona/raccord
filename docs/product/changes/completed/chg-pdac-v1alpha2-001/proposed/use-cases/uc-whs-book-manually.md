---
id: UC-WHS-BOOK-MANUALLY
type: use-case
title: "Book packing-list equipment manually"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors: []
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-WHS-DEFAULT-BOOKING-TARGET"
  - "BR-WHS-SPLIT-DETACHES-FROM-VIRTUAL-KIT"
  - "BR-WHS-NO-EARLY-CHECK-OUT"
  - "BR-WHS-BOOK-OUT-TAKES-RESERVATION"
  - "BR-WHS-BOOKING-OPTIONAL-FOR-RENTAL"
  - "BR-WHS-STATUS-INDEPENDENCE"
  - "BR-WHS-AUTOMATIC-PROGRESS"
  - "BR-WHS-EARLY-RETURN-RELEASES"
  - "BR-WHS-BOOKING-MARKS-READY"
  - "BR-WHS-CUSTOM-STATUS-OUT"
  - "BR-WHS-SALE-STOCK-ON-BOOKING"
uses-terms:
  - "TERM-WAREHOUSE-BOOKING"
  - "TERM-WAREHOUSE-STATUS"
  - "TERM-PACKING-LIST"
  - "TERM-CHECK-OUT"
  - "TERM-CHECK-IN"
provenance:
  source: ".private/research/help-center/articles/13901986840082.md, .private/research/help-center/articles/360015601560.md, .private/research/help-center/articles/14004547874834.md, .private/research/help-center/articles/13901986722322.md, .private/research/help-center/articles/360014382840.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Equipment moves between warehouse statuses without scanning.

## Trigger

Packing, loading or receiving a list.

## Preconditions

The list is visible on the board.

## Main Flow

1. The technician opens the list and chooses source and target statuses.
2. The product shows each line with planned quantity, quantity still to book and notes, grouped and sorted per display preference.
3. The technician books everything, a whole combination with content, one unit at a time, or a typed amount.
4. The product records the bookings and updates counts.
5. When everything reached a status, the product proposes to move the list or project status, which the technician may accept.

## Alternative Flows

- Split a line into separate booking lines.
- Search any planned article from within the list.
- Book backwards to undo a mistake.

## Failure Conditions

- Before the planning period starts: refused.

## Postconditions

Bookings recorded; derived project progress updated.
