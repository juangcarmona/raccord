---
id: "SB-DOC-ON-LOCATION-SLIP"
type: structured-behaviour
title: "An on-location slip lists only items still out"
status: draft
illustrates:
  - "BR-DOC-PACKING-STATE-FILTER"
given:
  - "A project has returned except some items left on location"
when: "A packing slip of prepped or on-location items is generated"
then:
  - "Only the items still out are listed"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360014240919.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Supports chasing partial returns.

## Boundaries

None.
