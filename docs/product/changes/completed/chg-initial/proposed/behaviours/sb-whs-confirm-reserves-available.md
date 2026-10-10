---
id: SB-WHS-CONFIRM-RESERVES-AVAILABLE
type: structured-behaviour
title: "Confirmation reserves what is available"
status: draft
illustrates:
  - "BR-WHS-AUTO-RESERVE-ON-CONFIRM"
  - "UC-WHS-RESERVE-EQUIPMENT"
given:
  - "5 units are owned"
  - "Another project has 2 reserved in the same period"
  - "A pending project plans 4 units"
when: "The pending project is confirmed"
then:
  - "3 units are reserved for it"
  - "1 unit remains a shortage"
  - "Its line is partially reserved"
uses-terms:
  - "TERM-RESERVATION"
  - "TERM-RESERVATION-STATE"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/13415175193490.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Automatic reservation is bounded by reservable stock.

## Boundaries

None.
