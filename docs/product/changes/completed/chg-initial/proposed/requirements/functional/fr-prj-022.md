---
id: FR-PRJ-022
type: functional-requirement
title: "Configure the task status workflow"
status: draft
derived-from:
  - "UC-PRJ-CONFIGURE-TASK-STATUSES"
  - "BR-PRJ-TASK-STATUS-ENDPOINTS"
  - "BR-PRJ-TASK-STATUS-DELETION"
  - "BR-PRJ-TASK-CALENDAR-SYNC"
  - "CON-PRJ-CALENDAR-TWO-STATES"
verification:
  - scenario-ref: "SB-PRJ-DELETE-STATUS-MOVES-TASKS"
  - scenario-ref: "SB-PRJ-REVIEW-STATUS-SYNCS-OPEN"
uses-terms:
  - "TERM-TASK-STATUS"
provenance:
  source: ".private/research/help-center/articles/38421530397202.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let administrators add, rename, recolour, reorder and delete task statuses around the permanent To do and Completed, reassigning tasks of deleted statuses, and MUST sync tasks to calendars as completed or not.

## Rationale

Workflows differ per company.
