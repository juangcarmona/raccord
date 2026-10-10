---
id: BR-WHS-CUSTOM-STATUS-OUT
type: business-rule
title: "Custom statuses count as out of the warehouse"
status: draft
applies-to:
  - "UC-WHS-CONFIGURE-WAREHOUSE-STATUSES"
  - "UC-WHS-BOOK-MANUALLY"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-CUSTOM-WAREHOUSE-STATUS"
provenance:
  source: ".private/research/help-center/articles/15239120279442.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Equipment in any custom warehouse status is treated as out of the warehouse and unavailable, and custom statuses never change a project's financial state.

## Rationale

A custom step means the gear is not on the shelf.

## Examples

Gear in cleaning cannot be booked out for another job.

## Exceptions

None.
