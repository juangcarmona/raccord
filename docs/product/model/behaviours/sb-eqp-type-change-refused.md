---
id: SB-EQP-TYPE-CHANGE-REFUSED
type: structured-behaviour
title: "Type change of planned equipment refused"
status: draft
illustrates:
  - "BR-EQP-TYPE-FIXED-ONCE-PLANNED"
given:
  - "A virtual kit has been planned on at least one project"
when: "The equipment manager tries to change the virtual kit into a single item"
then:
  - "The change is refused"
  - "The manager is directed to duplicate the item and archive the original"
uses-terms:
  - "TERM-EQUIPMENT-TYPE"
provenance:
  source: ".private/research/help-center/articles/360013718240.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Protects projects from structural changes of their equipment.

## Boundaries

None.
