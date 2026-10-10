---
id: SB-WHS-REALLOCATE-BETWEEN-PROJECTS
type: structured-behaviour
title: "Moving a reservation from one project to another"
status: draft
illustrates:
  - "BR-WHS-RESERVATION-PRIORITY"
  - "UC-WHS-REALLOCATE-RESERVATIONS"
given:
  - "2 units are owned"
  - "Project A has 1 reserved, project B needs 2 and has 1 reserved"
when: "The coordinator lowers A's reservation to 0 and sets B's reservation to 2"
then:
  - "B is fully reserved"
  - "A shows a shortage of 1"
  - "Reservable stock in the period is 0"
uses-terms:
  - "TERM-RESERVATION"
  - "TERM-RESERVABLE-STOCK"
provenance:
  source: ".private/research/help-center/articles/13415175193490.md, .private/research/help-center/articles/360013158280.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Reallocation is explicit and moves the shortage.

## Boundaries

None.
