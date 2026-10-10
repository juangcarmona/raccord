---
id: SB-CRW-DEFAULT-STATUS-CHANGE
type: structured-behaviour
title: "Changing the default plan status"
status: draft
illustrates:
  - "BR-CRW-DEFAULT-PLAN-STATUS-NEW-ONLY"
given:
  - "A project has two draft assignments"
when: "The planner sets the project's default plan status to final and plans a third crew member"
then:
  - "The two existing assignments remain draft"
  - "The new assignment is final"
uses-terms:
  - "TERM-PLAN-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014607700.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Defaults apply forward only.

## Boundaries

None.
