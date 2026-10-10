---
id: FR-CRW-FUNCTIONS
type: functional-requirement
title: "Define crew and transport functions"
status: draft
derived-from:
  - "UC-CRW-MANAGE-DEFAULT-FUNCTIONS"
  - "UC-CRW-ADD-FUNCTION-TO-PROJECT"
  - "UC-CRW-CREATE-FUNCTION-GROUP"
  - "BR-CRW-FUNCTION-KIND-PRICING"
  - "BR-CRW-FUNCTION-TIMING-PROPAGATION"
  - "BR-CRW-ONE-OFF-NOT-REUSABLE"
verification:
  - scenario: "Adding a default crew function with quantity 2 linked to the show period creates a project function that moves when the show period moves"
  - scenario: "A one-off transport function priced per kilometre is not offered in other projects"
uses-terms:
  - "TERM-PROJECT-FUNCTION"
  - "TERM-FUNCTION-GROUP"
provenance:
  source: ".private/research/help-center/articles/14232209807506.md, .private/research/help-center/articles/360013223940.md, .private/research/help-center/articles/14319493249170.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let planners add default or one-off crew and transport functions with quantity, timing mode, usage and planning periods, prices, costs and tags to projects, grouped in function groups linked to the time schedule.

## Rationale

Roles needed must be explicit before staffing.
