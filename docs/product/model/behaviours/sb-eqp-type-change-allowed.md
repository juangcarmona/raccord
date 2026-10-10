---
id: SB-EQP-TYPE-CHANGE-ALLOWED
type: structured-behaviour
title: "Type change of never-planned equipment"
status: draft
illustrates:
  - "BR-EQP-TYPE-FIXED-ONCE-PLANNED"
given:
  - "A virtual kit has never been planned"
when: "The equipment manager changes it into a physical kit and saves"
then:
  - "The item is now a physical kit"
  - "No copy is created"
uses-terms:
  - "TERM-EQUIPMENT-TYPE"
provenance:
  source: ".private/research/help-center/articles/360013718240.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Shows the free change before first use.

## Boundaries

None.
