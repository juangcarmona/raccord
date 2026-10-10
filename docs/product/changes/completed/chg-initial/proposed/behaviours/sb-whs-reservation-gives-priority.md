---
id: SB-WHS-RESERVATION-GIVES-PRIORITY
type: structured-behaviour
title: "Reserving gives one project priority"
status: draft
illustrates:
  - "BR-WHS-RESERVATION-PRIORITY"
  - "UC-WHS-RESERVE-EQUIPMENT"
given:
  - "1 DJ booth is owned"
  - "Two pending projects each plan 1 DJ booth in the same period"
  - "Both show a shortage of 1"
when: "The coordinator reserves the booth for the first project"
then:
  - "The first project is fully reserved and has no shortage"
  - "The second project keeps a shortage of 1"
uses-terms:
  - "TERM-RESERVATION"
  - "TERM-SHORTAGE"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/13415175193490.md, .private/research/help-center/articles/360017700639.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Reservation decides who is served.

## Boundaries

None.
