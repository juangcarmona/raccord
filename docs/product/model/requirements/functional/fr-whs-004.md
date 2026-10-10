---
id: FR-WHS-004
type: functional-requirement
title: "Per-line linked times"
status: draft
derived-from:
  - "UC-WHS-SET-LINE-TIMES"
  - "BR-WHS-LINE-TIMES-LINKED"
  - "BR-WHS-PHYSICAL-KIT-SHARED-TIME"
verification:
  - scenario-ref: "SB-WHS-STAGGERED-LINE-TIMES-FREE-STOCK"
  - scenario-ref: "SB-WHS-PHYSICAL-KIT-TIME-REFUSED"
uses-terms:
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let each planned line (singly or in bulk) carry its own planning and usage period chosen among time-schedule entries of the project, MUST allow creating a new entry in place, and MUST refuse deviating times for content of physical combinations.

## Rationale

Equipment is needed for parts of productions.
