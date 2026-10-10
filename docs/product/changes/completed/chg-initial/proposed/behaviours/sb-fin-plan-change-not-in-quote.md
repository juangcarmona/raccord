---
id: SB-FIN-PLAN-CHANGE-NOT-IN-QUOTE
type: structured-behaviour
title: "Plan change without regeneration"
status: draft
illustrates:
  - "BR-FIN-QUOTE-SNAPSHOTS-PLAN"
given:
  - "A quote was generated"
when: "The coordinator adds a projector to the project"
then:
  - "The existing quote still shows the earlier content"
  - "The projector appears only after regenerating"
uses-terms:
  - "TERM-QUOTE"
provenance:
  source: ".private/research/help-center/articles/360012594659.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Quotes are snapshots.

## Boundaries

None.
