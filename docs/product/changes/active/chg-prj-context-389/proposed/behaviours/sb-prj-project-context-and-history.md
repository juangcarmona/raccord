---
id: SB-PRJ-PROJECT-CONTEXT-AND-HISTORY
type: structured-behaviour
title: "Organize project context and review its history"
status: active
illustrates:
  - "UC-PRJ-ADD-NOTE"
  - "UC-PRJ-ATTACH-FILE"
  - "UC-PRJ-MANAGE-FILE-FOLDERS"
  - "UC-PRJ-DEFINE-CUSTOM-FIELD"
  - "UC-PRJ-SAVE-VIEW"
  - "UC-PRJ-TAG-RECORDS"
  - "UC-PRJ-REVIEW-HISTORY"
given:
  - "A synthetic project is visible to an operations coordinator"
  - "A workspace administrator may define project fields and saved views"
  - "The coordinator has rights to add notes, manage files and apply tags"
when: "The administrator defines a searchable project field and a saved view, then the coordinator adds a note, files an attachment in a folder, tags the project, filters by the tag and opens project history"
then:
  - "The project field and saved view are available according to their visibility settings"
  - "The note follows its selected visibility and display targets"
  - "The attachment is stored in the chosen folder and its access follows file rights"
  - "The project appears in the tag filter and saved view"
  - "Project history identifies the saved changes and who made them"
  - "A user outside the workspace cannot see the project, its files or its history"
uses-terms:
  - "TERM-PROJECT-NOTE"
  - "TERM-ATTACHED-FILE"
  - "TERM-FILE-FOLDER"
  - "TERM-CUSTOM-FIELD"
  - "TERM-SAVED-VIEW"
  - "TERM-TAG"
  - "TERM-HISTORY-LOG"
---

## Intent

Demonstrates a coherent, access-controlled project-context workflow across its modeled records and history.

## Boundaries

The scenario uses synthetic records. It does not cover document generation or template editing, which remain under #329–#330.
