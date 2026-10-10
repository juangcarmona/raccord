---
id: "SB-TRB-DAY-KIND-HIDES-PROJECT"
type: structured-behaviour
title: "Equipment-out selection hides a crew-only day"
status: draft
illustrates:
  - "BR-TRB-OVERVIEW-DAY-KIND-FILTER"
given:
  - "A project needs only crew on 3 July and has no equipment leaving that day"
when: "The coordinator opens the calendar for that day with only equipment-out days selected"
then:
  - "The project is not shown on that day"
uses-terms:
  - "TERM-PROJECT-DAY-KIND"
provenance:
  source: ".private/research/help-center/articles/360014514680.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Day-kind settings explain missing calendar entries.

## Boundaries

None.
