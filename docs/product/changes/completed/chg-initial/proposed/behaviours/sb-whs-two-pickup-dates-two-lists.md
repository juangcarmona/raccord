---
id: SB-WHS-TWO-PICKUP-DATES-TWO-LISTS
type: structured-behaviour
title: "Two pickup dates make two packing lists"
status: draft
illustrates:
  - "BR-WHS-PACKING-LIST-PER-DATE"
  - "UC-WHS-REVIEW-WAREHOUSE-BOARD"
given:
  - "A project's truss leaves Monday and projectors leave Wednesday, both returning Saturday"
when: "The project is marked ready to pack"
then:
  - "Outgoing lists exist for Monday and Wednesday"
  - "One return list exists for Saturday"
uses-terms:
  - "TERM-PACKING-LIST"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Packing lists follow movement moments.

## Boundaries

None.
