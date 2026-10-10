---
id: SB-EQP-TIMELINE-STOCK-CHANGE
type: structured-behaviour
title: "Stock changes on the timeline"
status: draft
illustrates:
  - "UC-WHS-VIEW-AVAILABILITY-TIMELINE"
given:
  - "An item has a delivery of 10 scheduled next month"
when: "The item's availability timeline is opened"
then:
  - "A stock increase marker shows 10 on the delivery date"
  - "Planned projects appear as separate bars with quantities"
uses-terms:
  - "TERM-STOCK-MOVEMENT"
provenance:
  source: ".private/research/help-center/articles/360017649560.md, .private/research/help-center/articles/360015957259.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Stock movements are visible next to demand.

## Boundaries

None.
