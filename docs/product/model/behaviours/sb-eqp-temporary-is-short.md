---
id: SB-EQP-TEMPORARY-IS-SHORT
type: structured-behaviour
title: "Temporary equipment starts as a shortage"
status: draft
illustrates:
  - "BR-WHS-TEMPORARY-IS-SHORT"
when: "A temporary item with quantity 2 is added to a project"
then:
  - "It shows a shortage of 2"
  - "The catalog is unchanged"
uses-terms:
  - "TERM-TEMPORARY-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/23347879967762.md, .private/research/help-center/articles/360017699799.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Temporary items have no own stock.

## Boundaries

None.
