---
id: BR-CRW-FUNCTION-KIND-PRICING
type: business-rule
title: "Crew functions are priced by time, transport functions by distance"
status: draft
applies-to:
  - "UC-CRW-ADD-FUNCTION-TO-PROJECT"
  - "UC-CRW-MANAGE-DEFAULT-FUNCTIONS"
uses-terms:
  - "TERM-FUNCTION-KIND"
  - "TERM-TRANSPORT-FUNCTION"
provenance:
  source: ".private/research/help-center/articles/14232209807506.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

A crew function's price and cost are hourly, fixed or both (or a crew rate); a transport function's price and cost are per kilometre, fixed or both, using the distance from the warehouse to the location. Break length applies to crew functions only.

## Rationale

People are paid for time, vehicles cost per distance.

## Examples

A van function of 120 km at 0.80 per km with a fixed 50 is priced 146.

## Exceptions

None.
