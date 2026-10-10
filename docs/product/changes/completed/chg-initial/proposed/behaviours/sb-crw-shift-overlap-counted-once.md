---
id: SB-CRW-SHIFT-OVERLAP-COUNTED-ONCE
type: structured-behaviour
title: "Function inside a shift is counted once"
status: draft
illustrates:
  - "BR-CRW-SHIFT-FUNCTION-OVERLAP-HOURS"
  - "BR-CRW-SHIFT-AVAILABILITY-BLOCKING"
given:
  - "A crew member is on a warehouse shift 10:00-20:00 that leaves them available for other functions"
when: "The crew member is also planned on a delivery function 14:00-15:00"
then:
  - "The crew member's planned hours total 10, not 11"
uses-terms:
  - "TERM-SHIFT"
provenance:
  source: ".private/research/help-center/articles/17290277763090.md, .private/research/help-center/articles/17155945414674.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

No double counting.

## Boundaries

None.
