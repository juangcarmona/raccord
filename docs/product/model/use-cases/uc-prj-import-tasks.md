---
id: UC-PRJ-IMPORT-TASKS
type: use-case
title: "Import tasks from another tool"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-TASK-IMPORT-MATCHING"
  - "BR-PRJ-TASK-IMPORT-SUBTASK-ROWS"
  - "BR-PRJ-TASK-IMPORT-EXCLUSIONS"
  - "BR-PRJ-IMPORT-RESULT-REPORT"
  - "BR-PRJ-IMPORT-TAG"
uses-terms:
  - "TERM-TASK"
  - "TERM-SUBTASK"
  - "TERM-TAG"
provenance:
  source: ".private/research/help-center/articles/39225660180626.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Existing task work is brought in with deadlines, assignees, statuses and subtasks.

## Trigger

The company moves from another task tool.

## Preconditions

Statuses, crew members and items to link already exist.

## Main Flow

1. The user uploads a spreadsheet or comma-separated file and states whether it has a header row.
2. The product proposes column matches; the user corrects them, maps subtask columns and maps priority values.
3. The product previews the first ten rows; the user imports.
4. The product reports imported and failed rows with reasons and tags the created tasks with the import's date and time.

## Alternative Flows

- Undo: filter by the import tag, select and delete the tasks.

## Failure Conditions

Unknown status, unknown assignee, formatted values, several sheets or too much data make rows fail.

## Postconditions

Tasks and subtasks exist, linked to their items.
