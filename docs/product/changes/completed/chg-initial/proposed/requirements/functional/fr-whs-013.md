---
id: FR-WHS-013
type: functional-requirement
title: "Availability timeline: stock and overbooking"
status: draft
derived-from:
  - "UC-WHS-VIEW-AVAILABILITY-TIMELINE"
  - "BR-WHS-AVAILABILITY-BY-PLANNING-PERIOD"
verification:
  - scenario-ref: "SB-WHS-INQUIRY-SOFT-HOLD"
uses-terms:
  - "TERM-RESERVATION-STATE"
provenance:
  source: ".private/research/help-center/articles/13369374381714.md, .private/research/help-center/articles/360021519799.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST show, for selected articles over a chosen period, the forecast and reservable stock, and MUST mark the moments where planned quantities exceed stock.

## Rationale

Visual planning.
