---
id: SB-CRW-JOB-SIGNUP-DESPITE-CONFLICT
type: structured-behaviour
title: "Job board sign-up despite a conflict"
status: draft
illustrates:
  - "BR-CRW-JOB-BOARD-IGNORES-CONFLICTS"
given:
  - "A crew member is planned on a function on Saturday"
when: "The crew member signs up for a posted job overlapping that function"
then:
  - "The sign-up is accepted"
  - "The resulting double booking is left for the planner"
uses-terms:
  - "TERM-JOB-POSTING"
provenance:
  source: ".private/research/help-center/articles/17152009951634.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intent

Self-service does not check conflicts.

## Boundaries

None.
