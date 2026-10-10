---
id: SB-PRJ-SUBTASK-ROWS-GROUPED
type: structured-behaviour
title: "Repeated rows form one task with subtasks"
status: draft
illustrates:
  - "BR-PRJ-TASK-IMPORT-SUBTASK-ROWS"
given:
  - "Three rows share name 'Design main stage', deadline and assignee"
  - "Their subtask columns are 'sketch' done, 'render' done, 'drawing' not done"
when: "The file is imported"
then:
  - "One task 'Design main stage' exists"
  - "It has three subtasks, two completed and one not"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/39225660180626.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Row grouping.

## Boundaries

None.
