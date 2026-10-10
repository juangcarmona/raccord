---
id: "SB-TRB-COLLEAGUE-CHANGE-APPEARS"
type: structured-behaviour
title: "A colleague's change appears live"
status: draft
illustrates:
  - "BR-TRB-LIVE-CHANGES-PUSHED"
  - "UC-TRB-SEE-LIVE-CHANGES"
given:
  - "Ann and Ben both have project 1042 open"
  - "Both live update connections are working"
when: "Ben changes the project's usage period and saves"
then:
  - "Ann's screen shows the new usage period without reloading"
uses-terms:
  - "TERM-LIVE-UPDATE-CONNECTION"
provenance:
  source: ".private/research/help-center/articles/360013988420.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Core live collaboration behaviour.

## Boundaries

None.
