---
id: FR-CRW-TRAVEL-TIME
type: functional-requirement
title: "Include travel time in calls"
status: draft
derived-from:
  - "UC-CRW-CHANGE-ASSIGNMENT-DETAILS"
  - "BR-CRW-TRAVEL-TIME-EXTENDS-CALL"
verification:
  - scenario-ref: "SB-CRW-ROUND-TRIP-TRAVEL"
  - scenario-ref: "SB-CRW-WAY-THERE-TRAVEL"
uses-terms:
  - "TERM-TRAVEL-OPTION"
provenance:
  source: ".private/research/help-center/articles/360016488759.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST add a function's travel time to the time communicated to each crew member according to their travel option.

## Rationale

Crew must know when to leave.
