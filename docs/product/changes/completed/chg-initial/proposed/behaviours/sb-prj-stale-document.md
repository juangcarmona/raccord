---
id: SB-PRJ-STALE-DOCUMENT
type: structured-behaviour
title: "A document does not change until regenerated"
status: draft
illustrates:
  - "BR-PRJ-DOCUMENT-REGENERATION"
given:
  - "A packing slip was generated with 10 items"
when: "The coordinator adds an item to the project and downloads the slip without regenerating"
then:
  - "The downloaded slip still lists 10 items"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360013838560.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Documents are snapshots.

## Boundaries

None.
