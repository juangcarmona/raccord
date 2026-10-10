---
id: BR-CRW-TRAVEL-TIME-EXTENDS-CALL
type: business-rule
title: "Travel options widen the communicated working time"
status: draft
applies-to:
  - "UC-CRW-CHANGE-ASSIGNMENT-DETAILS"
uses-terms:
  - "TERM-TRAVEL-OPTION"
provenance:
  source: ".private/research/help-center/articles/360016488759.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

For an assignment with a travel option, the function's travel time is added before the start (round trip or way there) and/or after the end (round trip or way back) of the time communicated to the crew member, with a note that travel is included; travel time is taken from the location distance only when the project location has a distance and the function takes travel time over from the location.

## Rationale

Crew need to know when to leave, not just when the job starts.

## Examples

See SB-CRW-ROUND-TRIP-TRAVEL.

## Exceptions

None.
