---
id: SB-PRJ-PLANNED-VS-ACTUAL-TASK-HOURS
type: structured-behaviour
title: "Record task time and review planned versus tracked hours"
status: active
illustrates:
  - "UC-PRJ-TRACK-TASK-TIME"
  - "UC-PRJ-REPORT-TASK-TIME"
given:
  - "A project has a task with 5 planned hours"
  - "A project member may record time on the task"
when: "The member records 3 hours and an authorized coordinator opens the tracked-hours view"
then:
  - "The task shows 5 planned hours and 3 tracked hours as distinct values"
  - "The coordinator can identify which task the tracked hours belong to"
  - "Reviewing the totals does not alter the source time registration"
uses-terms:
  - "TERM-TRACKED-TIME"
  - "TERM-TASK-PLANNED-TIME"
provenance:
  source: "Issue #388 acceptance criteria and UC-PRJ-TRACK-TASK-TIME / UC-PRJ-REPORT-TASK-TIME"
  confidence: high
  recovered-from: documentation
---

## Intent

Makes the planned-versus-tracked time workflow observable through the Projects interface.

## Boundaries

This scenario covers recording and read-only comparison of task hours. Payroll, invoicing and changes to time-accounting semantics remain outside the workflow.
