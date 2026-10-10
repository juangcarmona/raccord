---
id: SB-WHS-PHYSICAL-KIT-TIME-REFUSED
type: structured-behaviour
title: "Content of a physical case cannot deviate"
status: draft
illustrates:
  - "BR-WHS-PHYSICAL-KIT-SHARED-TIME"
  - "UC-WHS-SET-LINE-TIMES"
when: "The coordinator tries to give one content line of a physical combination its own planning period"
then:
  - "The change is refused"
  - "The content keeps the combination's periods"
uses-terms:
  - "TERM-PLANNING-PERIOD"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Physical kits travel together.

## Boundaries

None.
