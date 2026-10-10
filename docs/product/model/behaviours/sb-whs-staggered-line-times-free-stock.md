---
id: SB-WHS-STAGGERED-LINE-TIMES-FREE-STOCK
type: structured-behaviour
title: "Projectors needed later stay free for other jobs"
status: draft
illustrates:
  - "BR-WHS-AVAILABILITY-BY-PLANNING-PERIOD"
  - "UC-WHS-SET-LINE-TIMES"
given:
  - "A project runs Monday to Friday"
  - "Its truss line has a Monday-to-Friday planning period"
  - "The company owns 4 projectors"
when: "The coordinator links the line of 4 projectors to a Wednesday-to-Friday schedule entry"
then:
  - "Availability of projectors is 4 on Monday and Tuesday and 0 from Wednesday to Friday"
  - "Another job can be planned with the projectors on Monday and Tuesday without shortage"
uses-terms:
  - "TERM-PLANNING-PERIOD"
  - "TERM-EQUIPMENT-AVAILABILITY"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Per-line planning periods free stock for the time it is not needed.

## Boundaries

None.
