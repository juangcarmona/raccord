---
id: SB-FIN-HIDDEN-FINANCIALS-STILL-UPDATE
type: structured-behaviour
title: "Planner without financial visibility"
status: draft
illustrates:
  - "BR-FIN-FINANCIAL-VISIBILITY"
given:
  - "A user's role has financial visibility switched off"
when: "The user adds ten lights to a project"
then:
  - "The user sees no prices or financial overview"
  - "The project price is recalculated including the lights"
uses-terms:
  - "TERM-FINANCIAL-DETAILS"
provenance:
  source: ".private/research/help-center/articles/4409044768658.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Hiding is display-only.

## Boundaries

None.
