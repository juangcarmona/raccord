---
id: FR-PRJ-027
type: functional-requirement
title: "Import tasks"
status: draft
derived-from:
  - "UC-PRJ-IMPORT-TASKS"
  - "BR-PRJ-TASK-IMPORT-MATCHING"
  - "BR-PRJ-TASK-IMPORT-SUBTASK-ROWS"
  - "BR-PRJ-IMPORT-TAG"
  - "CON-PRJ-IMPORT-FILE-FORMAT"
verification:
  - scenario-ref: "SB-PRJ-TASK-IMPORT-UNKNOWN-STATUS"
  - scenario-ref: "SB-PRJ-SUBTASK-ROWS-GROUPED"
uses-terms:
  - "TERM-TASK"
provenance:
  source: ".private/research/help-center/articles/39225660180626.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST import tasks and subtasks from files, matching statuses, assignees, priorities and linked items as documented, tagging the import and reporting failures.

## Rationale

Migration from other tools.
