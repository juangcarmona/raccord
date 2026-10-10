---
id: SB-WHS-INQUIRY-SOFT-HOLD
type: structured-behaviour
title: "Inquiry lowers the forecast without reserving"
status: draft
illustrates:
  - "BR-WHS-STATUS-AVAILABILITY-EFFECT"
  - "UC-WHS-VIEW-AVAILABILITY-TIMELINE"
given:
  - "22 units of an article are owned"
  - "An inquiry project plans 4 units from 10 to 12 June"
when: "The coordinator views the article's availability for 10 to 12 June"
then:
  - "The availability forecast is 18"
  - "The reservable stock is 22"
  - "The inquiry's line is unreserved"
uses-terms:
  - "TERM-AVAILABILITY-FORECAST"
  - "TERM-RESERVABLE-STOCK"
  - "TERM-SOFT-HOLD"
provenance:
  source: ".private/research/help-center/articles/13369374381714.md, .private/research/help-center/articles/360013158280.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Distinguishes forecast from reservable stock.

## Boundaries

None.
