---
id: BR-WHS-RESERVATION-PRIORITY
type: business-rule
title: "Reserved quantities take precedence over unreserved demand"
status: draft
applies-to:
  - "UC-WHS-RESERVE-EQUIPMENT"
  - "UC-WHS-REALLOCATE-RESERVATIONS"
  - "UC-WHS-REVIEW-SHORTAGES"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-RESERVATION"
  - "TERM-SHORTAGE"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/13415175193490.md, .private/research/help-center/articles/360017700639.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When several projects plan the same article in overlapping planning periods and stock is insufficient, reserved quantities are covered first and the shortage is attributed to the unreserved demand; reserving more for one project can only use reservable stock, so increasing one reservation may require lowering another.

## Rationale

Gives the business an explicit lever to decide which job is served.

## Examples

See SB-WHS-RESERVATION-GIVES-PRIORITY and SB-WHS-REALLOCATE-BETWEEN-PROJECTS.

## Exceptions

None.
