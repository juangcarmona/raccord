---
id: SB-PRJ-SHIFT-TO-NEW-START
type: structured-behaviour
title: "Postponing to a new start keeps the structure"
status: draft
illustrates:
  - "BR-PRJ-SHIFT-TO-START"
  - "UC-PRJ-SHIFT-PROJECT-TIMES"
given:
  - "Planning period 17 Feb 10:00 to 19 Feb 18:00"
  - "Usage period 18 Feb 12:00 to 18 Feb 23:00"
when: "The coordinator shifts all periods to a new start of 29 Feb 12:00"
then:
  - "Planning period becomes 29 Feb 12:00 to 2 Mar 20:00"
  - "Usage period becomes 1 Mar 14:00 to 2 Mar 01:00"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/13992531637522.md, .private/research/help-center/articles/360017700399.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Relative offsets are preserved: every period moves by the same 12 days and 2 hours.

## Boundaries

None.
