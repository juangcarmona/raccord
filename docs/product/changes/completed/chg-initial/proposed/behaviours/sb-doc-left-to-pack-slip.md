---
id: "SB-DOC-LEFT-TO-PACK-SLIP"
type: structured-behaviour
title: "A left-to-pack slip lists only what remains in the warehouse"
status: draft
illustrates:
  - "BR-DOC-PACKING-STATE-FILTER"
given:
  - "Part of a project's equipment is already on its way"
  - "The rest is still in the warehouse"
when: "A left-to-pack packing slip is generated"
then:
  - "Only the items not yet packed are listed"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360014240919.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Supports split departures.

## Boundaries

None.
