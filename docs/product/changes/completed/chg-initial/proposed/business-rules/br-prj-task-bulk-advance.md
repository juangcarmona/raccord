---
id: BR-PRJ-TASK-BULK-ADVANCE
type: business-rule
title: "Bulk status advance requires identical statuses"
status: draft
applies-to:
  - "UC-PRJ-PROGRESS-TASK"
uses-terms:
  - "TERM-TASK-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013703139.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Several tasks can be advanced together only when they all hold the same status; they move to the next status in the workflow order.

## Rationale

The 'next' status must be the same for all.

## Examples

See SB-PRJ-BULK-ADVANCE-MIXED.

## Exceptions

None.
