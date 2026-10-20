---
id: SB-PRJ-SHIFT-NEGATIVE-OFFSET
type: structured-behaviour
title: "Shifting by a negative amount moves periods earlier"
status: active
illustrates:
  - "BR-PRJ-SHIFT-BY-OFFSET"
  - "UC-PRJ-SHIFT-PROJECT-TIMES"
given:
  - "Build runs 10 June 08:00 to 18:00"
  - "Show runs 11 June 20:00 to 23:00"
when: "The coordinator shifts both by -2 hours and applies"
then:
  - "Build runs 10 June 06:00 to 16:00"
  - "Show runs 11 June 18:00 to 21:00"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/13992531637522.md, .private/research/help-center/articles/360017700399.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Signed offsets.

## Boundaries

None.
