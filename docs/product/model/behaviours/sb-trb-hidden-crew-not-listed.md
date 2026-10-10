---
id: "SB-TRB-HIDDEN-CREW-NOT-LISTED"
type: structured-behaviour
title: "Crew member hidden from planning is not listed"
status: draft
illustrates:
  - "BR-CRW-PLANNABLE-FLAG"
  - "UC-CRW-PLAN-RESOURCE-ON-FUNCTION"
given:
  - "Crew member Tom was set not to be shown in planning while unavailable for a long period"
when: "The planner searches for Tom while staffing a function"
then:
  - "Tom is not listed"
  - "Setting Tom to be shown in planning makes him plannable again"
uses-terms:
  - "TERM-CREW-MEMBER"
provenance:
  source: ".private/research/help-center/articles/360015032959.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Hidden crew explains missing names.

## Boundaries

None.
