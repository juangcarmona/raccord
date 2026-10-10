---
id: "SB-TRB-CREW-TAG-FILTER-HIDES"
type: structured-behaviour
title: "Active tag filter hides a crew member"
status: draft
illustrates:
  - "BR-CRW-DYNAMIC-LIST-MEMBERSHIP"
  - "UC-CRW-PLAN-RESOURCE-ON-FUNCTION"
given:
  - "A tag filter 'audio' is active in the crew list"
  - "Crew member Ella has no 'audio' tag"
when: "The planner searches for Ella by name"
then:
  - "Ella is not listed"
  - "Clearing the tag filter lists her"
uses-terms:
  - "TERM-TAG"
provenance:
  source: ".private/research/help-center/articles/360015032959.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Filters combine with name search.

## Boundaries

None.
