---
id: SB-WHS-CATALOG-PRICE-CHANGE-NOT-PROPAGATED
type: structured-behaviour
title: "Catalog price changes do not reach planned lines"
status: draft
illustrates:
  - "BR-WHS-PLANNED-SNAPSHOT"
  - "UC-WHS-REFRESH-PLANNED-DATA"
given:
  - "A line was planned at 40 per day"
  - "The catalog price is changed to 45"
when: "The coordinator opens the project"
then:
  - "The line still shows 40"
  - "After refreshing that line it shows 45"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/23230026077586.md, .private/research/help-center/articles/360013765880.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Snapshot semantics.

## Boundaries

None.
