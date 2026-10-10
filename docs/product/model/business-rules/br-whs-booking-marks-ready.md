---
id: BR-WHS-BOOKING-MARKS-READY
type: business-rule
title: "Booking an unmarked project marks it ready"
status: draft
applies-to:
  - "UC-WHS-BOOK-MANUALLY"
  - "UC-WHS-BOOK-BY-SCANNING"
  - "UC-WHS-MARK-READY-TO-PACK"
uses-terms:
  - "TERM-READY-TO-PACK"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When someone books equipment of a project not marked ready to pack, the product warns first and, on continuing, marks the project ready to pack automatically.

## Rationale

Reality wins over the handover flag, but not silently.

## Examples

See SB-WHS-BOOKING-UNMARKED-SUBPROJECT.

## Exceptions

None.
