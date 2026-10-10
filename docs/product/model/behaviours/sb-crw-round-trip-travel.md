---
id: SB-CRW-ROUND-TRIP-TRAVEL
type: structured-behaviour
title: "Round-trip travel widens the communicated time"
status: draft
illustrates:
  - "BR-CRW-TRAVEL-TIME-EXTENDS-CALL"
  - "UC-CRW-CHANGE-ASSIGNMENT-DETAILS"
given:
  - "A function runs 09:00-18:00 with 30 minutes travel time taken over from the location"
when: "The planner sets the crew member's travel option to round trip"
then:
  - "The time communicated to the crew member is 08:30-18:30"
  - "The communication states that travel both ways is included"
uses-terms:
  - "TERM-TRAVEL-OPTION"
provenance:
  source: ".private/research/help-center/articles/360016488759.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Call times include travel.

## Boundaries

None.
