---
id: BR-CRW-DEFAULT-PLAN-STATUS-NEW-ONLY
type: business-rule
title: "The default plan status applies only to new assignments"
status: draft
applies-to:
  - "UC-CRW-SWITCH-PLAN-STATUS"
  - "UC-CRW-PLAN-RESOURCE-ON-FUNCTION"
uses-terms:
  - "TERM-PLAN-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014607700.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

The default plan status chosen for a project or planning context is applied to resources planned from then on; existing assignments keep their status.

## Rationale

Changing a default must not expose or hide existing planning.

## Examples

Switching the default to final leaves earlier draft assignments in draft.

## Exceptions

None.
