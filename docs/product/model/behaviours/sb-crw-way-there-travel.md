---
id: SB-CRW-WAY-THERE-TRAVEL
type: structured-behaviour
title: "Travel only on the way there"
status: draft
illustrates:
  - "BR-CRW-TRAVEL-TIME-EXTENDS-CALL"
given:
  - "A function runs 09:00-18:00 with 30 minutes travel time"
when: "The planner sets the travel option to only the way there"
then:
  - "The communicated time is 08:30-18:00"
uses-terms:
  - "TERM-TRAVEL-OPTION"
provenance:
  source: ".private/research/help-center/articles/360016488759.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intent

Only the chosen leg is added.

## Boundaries

None.
