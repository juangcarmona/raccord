---
id: SB-WHS-CROSS-DOCK-CHAIRS
type: structured-behaviour
title: "Chairs go straight from one job to the next"
status: draft
illustrates:
  - "BR-WHS-CROSS-DOCK-ELIGIBILITY"
  - "UC-WHS-REVIEW-CROSS-DOCKING"
given:
  - "A confirmed project returns 400 chairs at 10:00"
  - "Another confirmed project takes 350 chairs at 11:00"
when: "The overview is opened with a 12-hour window"
then:
  - "Chairs show 400 returning, 350 leaving and an overlap of 350"
  - "Only 50 chairs need shelving"
uses-terms:
  - "TERM-CROSS-DOCKING-OVERLAP"
provenance:
  source: ".private/research/help-center/articles/11884262854930.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Cross-docking quantity.

## Boundaries

None.
