---
id: SB-WHS-ALL-BACK-FINALIZES
type: structured-behaviour
title: "All returned finalizes the project"
status: draft
illustrates:
  - "BR-WHS-AUTOMATIC-PROGRESS"
given:
  - "A project has two return lists, one fully returned"
when: "The last item of the second list is booked to returned"
then:
  - "The project status becomes finalizing"
  - "Complete still requires a person"
uses-terms:
  - "TERM-WAREHOUSE-BOOKING"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Automatic progress end.

## Boundaries

None.
