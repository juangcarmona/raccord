---
id: SB-CRW-UPDATE-TO-DEFAULT-SELECTED-FIELDS
type: structured-behaviour
title: "Updating functions to defaults touches only chosen fields"
status: draft
illustrates:
  - "BR-CRW-DEFAULT-FUNCTION-COPY"
  - "UC-CRW-UPDATE-FUNCTIONS-TO-DEFAULTS"
given:
  - "A project was duplicated from last year with a stagehand function"
  - "The default stagehand function's cost and price were changed since"
when: "The planner updates the project's stagehand function to the default, choosing only the cost field"
then:
  - "The project function's cost becomes the new default cost"
  - "Its price and other fields stay as they were"
uses-terms:
  - "TERM-DEFAULT-FUNCTION"
provenance:
  source: ".private/research/help-center/articles/34615607831314.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Updates are selective and explicit.

## Boundaries

None.
