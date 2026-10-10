---
id: SB-WHS-UNRETURNED-BECOMES-DELAYED
type: structured-behaviour
title: "Unreturned equipment makes a list delayed"
status: draft
illustrates:
  - "BR-WHS-DELAYED-AUTOMATIC"
  - "UC-WHS-REVIEW-WAREHOUSE-BOARD"
given:
  - "A return list's planning period ended yesterday"
  - "2 of 20 items are still on location"
when: "The board is opened today"
then:
  - "The list appears as delayed"
  - "Its sync indicator shows equipment left behind"
uses-terms:
  - "TERM-DELAYED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/15239125718546.md, .private/research/help-center/articles/23280754011154.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Delay is derived.

## Boundaries

None.
