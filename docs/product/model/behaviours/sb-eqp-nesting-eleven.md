---
id: SB-EQP-NESTING-ELEVEN
type: structured-behaviour
title: "Eleventh nesting level refused"
status: draft
illustrates:
  - "BR-EQP-NESTING-LIMIT"
  - "CON-EQP-NESTING-DEPTH"
given:
  - "Kits are already nested 10 levels deep"
when: "Someone tries to place another kit inside the innermost kit"
then:
  - "The nesting is refused"
uses-terms:
  - "TERM-NESTED-KIT"
provenance:
  source: ".private/research/help-center/articles/19934846796306.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Fixes the depth limit.

## Boundaries

None.
