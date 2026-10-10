---
id: "SB-TRB-SEARCH-BY-ADDED-COLUMN"
type: structured-behaviour
title: "Searching on an added column"
status: draft
illustrates:
  - "BR-TRB-SEARCH-INCLUDES-SHOWN-COLUMNS"
given:
  - "The planner added the custom field 'skill' as a column in the crew list used while planning"
when: "The planner searches for 'rigging'"
then:
  - "Crew members whose skill field contains rigging are listed"
uses-terms:
  - "TERM-CUSTOM-FIELD"
provenance:
  source: ".private/research/help-center/articles/360015032959.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Columns make fields searchable.

## Boundaries

Without the column, the same search only matches names.
