---
id: SB-CRW-IMPORT-DEFAULT-ROLE
type: structured-behaviour
title: "Imported crew get the default role"
status: draft
illustrates:
  - "BR-CRW-IMPORT-EXCLUSIONS"
when: "The planner imports a crew file containing a role column"
then:
  - "Every imported crew member has the default user role"
uses-terms:
  - "TERM-RESOURCE-IMPORT"
provenance:
  source: ".private/research/help-center/articles/360014366299.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Roles are set individually.

## Boundaries

None.
