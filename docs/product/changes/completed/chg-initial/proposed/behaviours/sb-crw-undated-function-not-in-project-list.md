---
id: SB-CRW-UNDATED-FUNCTION-NOT-IN-PROJECT-LIST
type: structured-behaviour
title: "An undated function only appears in the cross-project planner"
status: draft
illustrates:
  - "BR-CRW-FUNCTIONS-WITHOUT-TIMES"
given:
  - "A function was added with a duration and no dates"
when: "The planner opens the project's crew scheduling list"
then:
  - "The function is not listed"
  - "It appears among unscheduled functions in the cross-project planner"
uses-terms:
  - "TERM-FUNCTION-TIMING-MODE"
provenance:
  source: ".private/research/help-center/articles/360014594620.md, .private/research/help-center/articles/360014625560.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Where undated work lives.

## Boundaries

None.
