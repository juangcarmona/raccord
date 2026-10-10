---
id: BR-CRW-RETIME-WITH-PLANNED-RESOURCES
type: business-rule
title: "Retiming a staffed function asks what happens to planned resources"
status: draft
applies-to:
  - "UC-CRW-RETIME-FUNCTION"
  - "UC-CRW-RETIME-FUNCTION-GROUP"
uses-terms:
  - "TERM-CREW-ASSIGNMENT"
  - "TERM-CREW-PLANNING-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360014594620.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

When the planning period of a function with planned resources changes, the planner chooses to apply the new period to all planned crew and vehicles, keep their current periods, or remove them; for a function group a fourth choice decides per function.

## Rationale

Existing commitments must not change silently.

## Examples

See SB-CRW-RETIME-KEEP-PLANNED and SB-CRW-RETIME-REMOVE-PLANNED.

## Exceptions

None.
