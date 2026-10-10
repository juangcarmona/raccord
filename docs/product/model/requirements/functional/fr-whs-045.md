---
id: FR-WHS-045
type: functional-requirement
title: "Availability timeline: other stock effects"
status: draft
derived-from:
  - "UC-WHS-VIEW-AVAILABILITY-TIMELINE"
  - "BR-WHS-STATUS-AVAILABILITY-EFFECT"
  - "BR-WHS-AVAILABILITY-BY-PLANNING-PERIOD"
verification:
  - scenario-ref: "SB-EQP-TIMELINE-STOCK-CHANGE"
uses-terms:
  - "TERM-RESERVATION-STATE"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/360012699859.md"
  confidence: medium
  recovered-from: documentation
---

## Requirement

The product MUST show on the availability timeline stock changes such as purchases and write-offs that alter the available quantity.

## Rationale

Split from FR-WHS-013 so each timeline obligation can be verified on its own.
