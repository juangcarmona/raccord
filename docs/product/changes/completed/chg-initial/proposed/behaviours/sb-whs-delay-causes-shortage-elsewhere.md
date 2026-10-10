---
id: SB-WHS-DELAY-CAUSES-SHORTAGE-ELSEWHERE
type: structured-behaviour
title: "Delayed return creates a shortage on the next job"
status: draft
illustrates:
  - "BR-WHS-INVALIDATION-ON-STOCK-CHANGE"
  - "UC-WHS-MARK-DELAYED"
given:
  - "5 speakers are on location for project A whose planning period ends Monday"
  - "Project B reserves 5 speakers from Tuesday"
when: "Speakers of A are marked delayed until Wednesday"
then:
  - "The speakers stay unavailable until Wednesday"
  - "Project B shows an invalid reservation shortage of 5 for Tuesday"
uses-terms:
  - "TERM-DELAYED-EQUIPMENT"
  - "TERM-INVALID-RESERVATION"
provenance:
  source: ".private/research/help-center/articles/360012699859.md, .private/research/help-center/articles/23280754011154.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Delays propagate truthfully.

## Boundaries

None.
