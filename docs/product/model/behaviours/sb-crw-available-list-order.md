---
id: SB-CRW-AVAILABLE-LIST-ORDER
type: structured-behaviour
title: "Reserved crew top the available list"
status: draft
illustrates:
  - "BR-CRW-AVAILABLE-LIST-ORDER"
given:
  - "For a function's time one crew member is reserved, one declared available and one unknown"
when: "The planner opens the available list for the function"
then:
  - "The order is the reserved member, then the available member, then the unknown member"
uses-terms:
  - "TERM-CREW-LIST"
provenance:
  source: ".private/research/help-center/articles/12866138319506.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Reserved people first.

## Boundaries

None.
