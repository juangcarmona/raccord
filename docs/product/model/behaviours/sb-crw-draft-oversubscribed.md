---
id: SB-CRW-DRAFT-OVERSUBSCRIBED
type: structured-behaviour
title: "More draft acceptances than needed"
status: draft
illustrates:
  - "BR-CRW-DRAFT-INVITATION-EFFECT"
given:
  - "A function needs 2 people"
  - "Five crew members received a draft-planning invitation"
when: "Four crew members answer yes"
then:
  - "All four are planned in draft, invisible to them"
  - "The planner can keep two and remove the others"
uses-terms:
  - "TERM-DRAFT-PLANNING"
provenance:
  source: ".private/research/help-center/articles/17080436056978.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Planner chooses among volunteers.

## Boundaries

None.
