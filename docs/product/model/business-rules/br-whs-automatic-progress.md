---
id: BR-WHS-AUTOMATIC-PROGRESS
type: business-rule
title: "Warehouse events drive project progress"
status: draft
applies-to:
  - "UC-WHS-BOOK-MANUALLY"
  - "UC-WHS-BOOK-BY-SCANNING"
  - "BC-PROJECTS"
uses-terms:
  - "TERM-WAREHOUSE-BOOKING"
  - "TERM-PROJECT-STATUS"
  - "TERM-PROJECT-STATUS-IN-PROGRESS"
  - "TERM-PROJECT-STATUS-EXECUTION-DONE"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A project's management status becomes in progress automatically when its first item is booked in the warehouse and finalizing when all equipment of every packing list is back; complete is set by a person.

## Rationale

Progress should not need double entry.

## Examples

See SB-WHS-FIRST-BOOKING-STARTS-PROJECT.

## Exceptions

None.
