---
id: SB-WHS-LONG-CODE-TRUNCATED
type: structured-behaviour
title: "Codes longer than 25 characters collide"
status: draft
illustrates:
  - "BR-WHS-SCAN-CODE-UNIQUE"
given:
  - "Two labels differ only after the 25th character"
when: "Both are assigned as codes"
then:
  - "Both are cut to 25 characters"
  - "They become identical and cannot identify distinct articles"
uses-terms:
  - "TERM-SCAN-CODE"
provenance:
  source: ".private/research/help-center/articles/360013101260.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Hard length limit.

## Boundaries

None.
