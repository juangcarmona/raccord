---
id: FR-CRW-PLAN-VISIBILITY
type: functional-requirement
title: "Draft and final planning"
status: draft
derived-from:
  - "UC-CRW-SWITCH-PLAN-STATUS"
  - "BR-CRW-DRAFT-INVISIBLE"
  - "BR-CRW-DEFAULT-PLAN-STATUS-NEW-ONLY"
verification:
  - scenario-ref: "SB-CRW-DRAFT-HIDDEN-FROM-CREW"
  - scenario-ref: "SB-CRW-FINALISE-FUNCTION"
  - scenario-ref: "SB-CRW-DEFAULT-STATUS-CHANGE"
uses-terms:
  - "TERM-PLAN-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014607700.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST keep draft assignments invisible to crew, show final ones in their schedules, switch status per assignment or function, and apply a default plan status to new assignments only.

## Rationale

Plan in private, publish when ready.
