---
id: FR-CRW-101
type: functional-requirement
title: "Adjust function times on the cross-project timeline"
status: draft
derived-from:
  - "UC-CRW-ADJUST-FUNCTION-TIMES-IN-PLANNER"
  - "BR-CRW-PLANNER-MOVE-UPDATES-PROJECT"
  - "BR-CRW-FUNCTIONS-WITHOUT-TIMES"
verification:
  - scenario-ref: "SB-CRW-PLANNER-DRAG-PRICE-UNCHANGED"
uses-terms:
  - "TERM-PROJECT-FUNCTION"
  - "TERM-FUNCTION-TIMING-MODE"
provenance:
  source: ".private/research/help-center/articles/360014625560.md, .private/research/help-center/articles/360014610140.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a planner move a function, change its start or end, or move several selected functions together directly on the timeline spanning projects, and MUST write the resulting times back to the owning projects. The product MUST allow a function that has no times yet to be placed on the timeline so that it receives a date, MUST leave prices unchanged by such moves, and MUST reflect any cost difference in the project financials.

## Rationale

Plans shift constantly and planners need to react where they see all commitments, without opening each project.
