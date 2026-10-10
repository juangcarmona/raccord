---
id: BR-CRW-PLANNER-MOVE-UPDATES-PROJECT
type: business-rule
title: "Moving a function in the planner changes the project but not its price"
status: draft
applies-to:
  - "UC-CRW-ADJUST-FUNCTION-TIMES-IN-PLANNER"
uses-terms:
  - "TERM-PROJECT-FUNCTION"
  - "TERM-PLANNED-COST"
provenance:
  source: ".private/research/help-center/articles/360014625560.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Moving, extending or shortening a function in the cross-project planning view updates its times in the project; the project price is not changed and the cost effect is shown in the project's financial overview.

## Rationale

Planning is fast while pricing stays deliberate.

## Examples

Extending a rigging function by two hours raises planned cost only.

## Exceptions

None.
