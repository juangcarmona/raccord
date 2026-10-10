---
id: SB-WHS-MARK-DELAYED-EXTENDS
type: structured-behaviour
title: "Marking delayed extends planning but not charge"
status: draft
illustrates:
  - "BR-WHS-DELAY-EXTENDS-PLANNING-ONLY"
  - "UC-WHS-MARK-DELAYED"
given:
  - "Group Audio of a project ended Monday 18:00 with 3 items on location"
when: "The technician marks Audio delayed until Thursday 10:00"
then:
  - "The planning period of the 3 items ends Thursday 10:00"
  - "The usage period and price are unchanged"
  - "From Thursday 10:00 the items are available again unless still unreturned"
uses-terms:
  - "TERM-EXPECTED-RETURN-DATE"
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360012699859.md, .private/research/help-center/articles/23280754011154.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Separates availability from billing.

## Boundaries

None.
