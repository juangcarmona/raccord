---
id: SB-CRW-DOUBLE-BOOKING-AFTER-SHIFT
type: structured-behaviour
title: "Shifting a project creates a double booking"
status: draft
illustrates:
  - "UC-CRW-RESOLVE-PLANNING-CONFLICTS"
given:
  - "A crew member is planned on two projects on consecutive days"
when: "The project's time schedule is moved one day earlier, overlapping another project where the crew member is planned"
then:
  - "The crew member is planned on two overlapping functions"
  - "The double booking is visible to the planner"
uses-terms:
  - "TERM-PLANNING-CONFLICT"
provenance:
  source: ".private/research/help-center/articles/14488851931026.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intent

Shows how conflicts arise.

## Boundaries

None.
