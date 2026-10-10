---
id: SB-CRW-EARLY-CALL-KEEPS-PRICE
type: structured-behaviour
title: "Calling a technician earlier keeps the price"
status: draft
illustrates:
  - "BR-CRW-USAGE-VERSUS-PLANNING-PERIOD"
  - "UC-CRW-RETIME-FUNCTION"
given:
  - "A function is charged 14:00-18:00"
  - "A technician is planned on it"
when: "The planner sets the technician's planning period to 13:30-18:30"
then:
  - "The function's price for the customer is unchanged"
  - "The planned cost includes the extra hour"
  - "The technician's schedule shows 13:30-18:30"
uses-terms:
  - "TERM-CREW-PLANNING-PERIOD"
  - "TERM-FUNCTION-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360014594620.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Separates customer price from crew occupancy.

## Boundaries

None.
