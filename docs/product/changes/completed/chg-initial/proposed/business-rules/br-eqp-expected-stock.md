---
id: BR-EQP-EXPECTED-STOCK
type: business-rule
title: "Expected warehouse stock calculation"
status: draft
applies-to:
  - "UC-EQP-COUNT-SELECTED-EQUIPMENT"
uses-terms:
  - "TERM-EXPECTED-WAREHOUSE-STOCK"
provenance:
  source: ".private/research/help-center/articles/360015960120.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The expected warehouse stock of a counted item equals its quantity minus the units that are content of physical kits or on location for projects at the creation time of the count.

## Rationale

Only what should be on the shelves can be counted.

## Examples

- 10 microphones, 2 inside cases, 3 on jobs: expected 5.

## Exceptions

None.
