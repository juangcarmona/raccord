---
id: "BR-DOC-PACKING-STATE-FILTER"
type: business-rule
title: "Packing slips can be limited by packing state"
status: draft
applies-to:
  - "UC-DOC-GENERATE-DOCUMENT"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360014240919.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A packing slip lists either all items, only items currently prepped or on location, or only items still left to pack, according to the kind of slip.

## Rationale

Supports partial departures and partial returns with dedicated packing slips.

## Examples

See SB-DOC-LEFT-TO-PACK-SLIP and SB-DOC-ON-LOCATION-SLIP.

## Exceptions

None.
