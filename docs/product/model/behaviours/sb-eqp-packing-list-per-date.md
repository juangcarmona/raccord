---
id: SB-EQP-PACKING-LIST-PER-DATE
type: structured-behaviour
title: "Separate packing lists per pickup date"
status: draft
illustrates:
  - "BR-WHS-PACKING-LIST-PER-DATE"
given:
  - "A project plans projectors with their own planning period starting on day three"
  - "Other equipment follows the group period starting on day one"
when: "The project is made ready to pack"
then:
  - "Two packing lists exist for the project, one per pickup date"
  - "Each carries the project name"
uses-terms:
  - "TERM-PACKING-LIST"
  - "TERM-PLANNING-PERIOD"
provenance:
  source: ".private/research/help-center/articles/39128596992658.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Crews pack per day of departure.

## Boundaries

None.
