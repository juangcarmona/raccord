---
id: FR-WHS-103
type: functional-requirement
title: "Return found equipment to stock"
status: draft
derived-from:
  - "UC-WHS-MARK-FOUND"
  - "BR-WHS-LOSS-REDUCES-STOCK"
verification:
  - scenario-ref: "SB-WHS-FOUND-RESTORES"
uses-terms:
  - "TERM-LOST-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360014392200.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a technician mark a lost equipment record as found with the date found and, on confirmation, MUST count the recovered units back into stock so that they are available again.

## Rationale

A recovered unit should be bookable again straight away.
