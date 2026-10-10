---
id: SB-INT-FIELD-TRANSITION
type: structured-behaviour
title: "Moved field served twice during transition"
status: draft
illustrates:
  - "BR-INT-DEPRECATION-TRANSITION"
given:
  - "planning period fields are moving from equipment groups to planned equipment"
  - "the transition period has not ended"
  - "no individual equipment times were set"
when: "an application reads the planning period of the group and of its planned equipment"
then:
  - "both return the same start and end"
uses-terms:
  - "TERM-API-DEPRECATION"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Intent

No break during deprecation.

## Boundaries

None.
