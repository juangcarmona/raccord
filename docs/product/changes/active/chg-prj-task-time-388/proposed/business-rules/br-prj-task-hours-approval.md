---
id: BR-PRJ-TASK-HOURS-APPROVAL
type: business-rule
title: "Task hours follow the approval process"
status: active
applies-to:
  - "UC-PRJ-TRACK-TASK-TIME"
uses-terms:
  - "TERM-TRACKED-TIME"
  - "TERM-TIME-REGISTRATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/38423178380050.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Hours registered on tasks are pending until approved, like hours on functions and shifts; an approved registration is read-only until set back to pending.

## Rationale

Consistent control of all hours.

## Examples

A designer's 6 hours stay pending until the manager approves.

## Exceptions

None.
