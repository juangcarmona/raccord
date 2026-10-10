---
id: BR-CRW-SHIFT-FUNCTION-OVERLAP-HOURS
type: business-rule
title: "Hours overlapping a shift and a function count once"
status: draft
applies-to:
  - "UC-CRW-PLAN-RESOURCE-ON-SHIFT"
  - "UC-CRW-PLAN-RESOURCE-ON-FUNCTION"
uses-terms:
  - "TERM-SHIFT"
provenance:
  source: ".private/research/help-center/articles/17290277763090.md, .private/research/help-center/articles/17155945414674.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

When a crew member's function overlaps their shift, the shift's duration is ignored for the overlapping period so the hours are not counted twice.

## Rationale

Accurate hours and costs.

## Examples

See SB-CRW-SHIFT-OVERLAP-COUNTED-ONCE.

## Exceptions

None.
