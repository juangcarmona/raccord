---
id: BR-PRJ-TIME-REGISTRATION-SYNC
type: business-rule
title: "Adjusting a time registration adjusts the task"
status: active
applies-to:
  - "UC-PRJ-TRACK-TASK-TIME"
uses-terms:
  - "TERM-TRACKED-TIME"
  - "TERM-TIME-REGISTRATION"
provenance:
  source: ".private/research/help-center/articles/38423178380050.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Creating a time registration from tracked task time is optional; when details are adjusted in the registration, the task's tracked time is adjusted accordingly.

## Rationale

One truth for hours.

## Examples

Reducing 3h to 2.5h in the registration sets the task to 2.5h.

## Exceptions

None.
