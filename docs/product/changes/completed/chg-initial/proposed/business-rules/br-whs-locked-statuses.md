---
id: BR-WHS-LOCKED-STATUSES
type: business-rule
title: "Three warehouse statuses are fixed"
status: draft
applies-to:
  - "UC-WHS-CONFIGURE-WAREHOUSE-STATUSES"
  - "BC-WAREHOUSE"
uses-terms:
  - "TERM-WAREHOUSE-STATUS"
  - "TERM-CUSTOM-WAREHOUSE-STATUS"
provenance:
  source: ".private/research/help-center/articles/15239120279442.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Confirmed, on location and returned always exist and keep their positions; custom statuses can be added, ordered by dragging between them and removed only when no project, packing list, equipment or container uses them.

## Rationale

Availability and delay logic depend on those anchors.

## Examples

Adding a cleaning status after returned is allowed; deleting a status still holding a packing list is refused.

## Exceptions

None.
