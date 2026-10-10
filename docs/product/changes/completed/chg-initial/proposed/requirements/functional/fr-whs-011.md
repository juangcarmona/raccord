---
id: FR-WHS-011
type: functional-requirement
title: "Compute availability per period"
status: draft
derived-from:
  - "BR-WHS-AVAILABILITY-BY-PLANNING-PERIOD"
  - "BR-WHS-STATUS-AVAILABILITY-EFFECT"
verification:
  - scenario-ref: "SB-WHS-INQUIRY-SOFT-HOLD"
uses-terms:
  - "TERM-EQUIPMENT-AVAILABILITY"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/13369374381714.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST compute availability per article and moment from owned stock, planned lines per project status over planning periods, reservations, losses and warehouse movements, as both a forecast and reservable stock.

## Rationale

Single source of stock truth.
