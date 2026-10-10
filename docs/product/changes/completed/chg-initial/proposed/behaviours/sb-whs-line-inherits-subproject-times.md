---
id: SB-WHS-LINE-INHERITS-SUBPROJECT-TIMES
type: structured-behaviour
title: "New lines take project defaults"
status: draft
illustrates:
  - "BR-WHS-LINE-TIMES-DEFAULT"
  - "UC-WHS-PLAN-EQUIPMENT"
given:
  - "A project has default periods 'Build day' and 'Show days'"
when: "The coordinator plans a new article on the project"
then:
  - "The line's planning period is 'Build day'"
  - "The line's usage period is 'Show days'"
uses-terms:
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Defaults avoid manual entry.

## Boundaries

None.
