---
id: SB-PRJ-CANCEL-WITHOUT-ACTIONS-WARNS
type: structured-behaviour
title: "Cancelling without actions keeps a warning"
status: active
illustrates:
  - "BR-PRJ-CANCEL-OPEN-ITEMS-WARNING"
  - "UC-PRJ-CANCEL-PROJECT"
given:
  - "A project has planned crew and an open task"
when: "The coordinator cancels it and chooses not to perform any action"
then:
  - "The project is Cancelled"
  - "Crew stays planned and the task stays open"
  - "A warning about open items is shown on the project"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/28083930405778.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Declining actions is allowed but visible.

## Boundaries

None.
