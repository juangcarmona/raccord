---
id: BR-EQP-HIDDEN-ITEM-STILL-BOOKABLE
type: business-rule
title: "Items hidden from planning remain bookable"
status: draft
applies-to:
  - "UC-EQP-CONFIGURE-ITEM-DETAILS"
  - "UC-EQP-ARCHIVE-ITEM"
  - "UC-WHS-ADD-ADDITIONAL-EQUIPMENT"
uses-terms:
  - "TERM-PLANNER-VISIBILITY"
provenance:
  source: ".private/research/help-center/articles/13141562920338.md, .private/research/help-center/articles/360013852060.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An item whose planner visibility is off is not offered when planning but stays in stock and can still be booked out in the warehouse as additional equipment.

## Rationale

Hiding is a softer retirement than archiving.

## Examples

- An old spare cable hidden from planning can still be scanned onto a job.

## Exceptions

None.
