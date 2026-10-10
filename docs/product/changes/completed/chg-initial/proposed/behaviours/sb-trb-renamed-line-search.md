---
id: "SB-TRB-RENAMED-LINE-SEARCH"
type: structured-behaviour
title: "Renamed line is found by its new name"
status: draft
illustrates:
  - "BR-TRB-PLANNED-LINE-NAME-EDITABLE"
given:
  - "A colleague renamed the planned line 'Spot 575' to 'Stage left wash' in the project"
when: "The coordinator searches the project for 'Spot'"
then:
  - "No line is found"
  - "Searching for 'Stage left wash' finds the line"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360015031799.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Searches see what colleagues renamed.

## Boundaries

None.
