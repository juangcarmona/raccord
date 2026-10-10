---
id: SB-EQP-ACCESSORY-SKIP-PRESENT
type: structured-behaviour
title: "Accessory already planned is not doubled"
status: draft
illustrates:
  - "BR-EQP-ACCESSORY-OPTIONS"
given:
  - "A rain cover accessory is automatic and set to skip if already present"
  - "A rain cover is already planned on the project"
when: "The planner plans another moving head"
then:
  - "No additional rain cover line or quantity is added"
uses-terms:
  - "TERM-ACCESSORY"
provenance:
  source: ".private/research/help-center/articles/360013703140.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Fixes the skip option.

## Boundaries

None.
