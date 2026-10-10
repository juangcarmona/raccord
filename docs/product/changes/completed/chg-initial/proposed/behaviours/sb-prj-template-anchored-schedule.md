---
id: SB-PRJ-TEMPLATE-ANCHORED-SCHEDULE
type: structured-behaviour
title: "Template schedule anchored to chosen date"
status: draft
illustrates:
  - "BR-PRJ-TEMPLATE-START-ANCHOR"
  - "UC-PRJ-CREATE-FROM-TEMPLATE"
given:
  - "A template has build day 1, show day 2 and break day 3 relative to its start"
when: "A coordinator creates a project from it with start date 5 September"
then:
  - "Build is on 5 September"
  - "Show is on 6 September"
  - "Break is on 7 September"
  - "Times of day can then be edited per period"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360013367659.md, .private/research/help-center/articles/360013590720.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Templates are date-independent.

## Boundaries

None.
