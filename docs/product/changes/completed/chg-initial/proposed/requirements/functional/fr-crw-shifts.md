---
id: FR-CRW-SHIFTS
type: functional-requirement
title: "Plan internal shifts"
status: draft
derived-from:
  - "UC-CRW-CREATE-SHIFT"
  - "UC-CRW-MANAGE-DEFAULT-SHIFTS"
  - "UC-CRW-EDIT-SHIFT"
  - "UC-CRW-PLAN-RESOURCE-ON-SHIFT"
  - "BR-CRW-SHIFT-AVAILABILITY-BLOCKING"
  - "BR-CRW-SHIFT-FUNCTION-OVERLAP-HOURS"
  - "BR-CRW-DEFAULT-SHIFT-COPY"
  - "BR-CRW-RECURRING-SHIFT-PLANNING"
verification:
  - scenario-ref: "SB-CRW-SHIFT-OVERLAP-COUNTED-ONCE"
  - scenario-ref: "SB-CRW-BLOCKING-SHIFT-UNAVAILABLE"
  - scenario-ref: "SB-CRW-DEFAULT-SHIFT-OVERRIDE"
  - scenario-ref: "SB-CRW-DELETE-ALL-RECURRING-SHIFTS"
  - scenario-ref: "SB-CRW-RECURRING-SHIFT-WITHOUT-END"
uses-terms:
  - "TERM-SHIFT"
  - "TERM-DEFAULT-SHIFT"
provenance:
  source: ".private/research/help-center/articles/17155945414674.md, .private/research/help-center/articles/17290277763090.md, .private/research/help-center/articles/17297296257426.md, .private/research/help-center/articles/17592602608018.md, .private/research/help-center/articles/26061527515538.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST support one-off, recurring and default-based shifts for non-project work with crew and vehicles planned on them, optional availability blocking and no double counting of overlapping hours.

## Rationale

Internal work needs planning too.
