---
id: SB-WHS-DOUBLE-BOOKED-SINGLE-UNIT
type: structured-behaviour
title: "Two overlapping plans of a single unit"
status: draft
illustrates:
  - "BR-WHS-SHORTAGE-DETECTION"
  - "UC-WHS-PLAN-EQUIPMENT"
given:
  - "1 unit is owned"
  - "A project plans 1 unit from 1 to 3 August"
when: "A second project plans 1 unit from 2 to 4 August"
then:
  - "Both projects show a shortage of 1 for the article"
  - "The timeline marks 2 to 3 August as overbooked"
uses-terms:
  - "TERM-SHORTAGE"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/360017700639.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Shortages are per overlapping interval, attributed to unreserved demand.

## Boundaries

None.
