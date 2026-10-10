---
id: SB-CRW-DEFAULT-SHIFT-OVERRIDE
type: structured-behaviour
title: "Changing one shift leaves the default untouched"
status: draft
illustrates:
  - "BR-CRW-DEFAULT-SHIFT-COPY"
given:
  - "A default shift has quantity 2"
  - "Shifts were created from it"
when: "The planner sets one shift's quantity to 3"
then:
  - "That shift needs 3 people"
  - "The default shift and other shifts keep quantity 2"
uses-terms:
  - "TERM-DEFAULT-SHIFT"
provenance:
  source: ".private/research/help-center/articles/17290277763090.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Templates are copied.

## Boundaries

None.
