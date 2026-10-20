---
id: BR-PRJ-TASK-STATUS-DELETION
type: business-rule
title: "Deleting a status moves its tasks"
status: active
applies-to:
  - "UC-PRJ-CONFIGURE-TASK-STATUSES"
uses-terms:
  - "TERM-TASK-STATUS"
provenance:
  source: ".private/research/help-center/articles/38421530397202.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Deleting a task status that tasks still hold requires choosing a replacement status, to which those tasks are moved.

## Rationale

No task may be left without a status.

## Examples

See SB-PRJ-DELETE-STATUS-MOVES-TASKS.

## Exceptions

None.
