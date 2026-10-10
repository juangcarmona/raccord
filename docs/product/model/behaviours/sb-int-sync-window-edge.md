---
id: SB-INT-SYNC-WINDOW-EDGE
type: structured-behaviour
title: "Item beyond six months not in the feed"
status: draft
illustrates:
  - "BR-INT-SYNC-WINDOW-SIX-MONTHS"
given:
  - "the user is planned on projects five and seven months ahead"
when: "the calendar application refreshes the feed"
then:
  - "the project in seven months is absent"
  - "the project in five months is present"
uses-terms:
  - "TERM-SYNC-WINDOW"
provenance:
  source: ".private/research/help-center/articles/360014047779.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Window boundary.

## Boundaries

Whether the window is centred or split differently between past and future is not stated.
