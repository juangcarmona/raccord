---
id: BR-WHS-PLANNED-SNAPSHOT
type: business-rule
title: "Planned equipment is a snapshot independent of the catalog"
status: draft
applies-to:
  - "UC-WHS-PLAN-EQUIPMENT"
  - "UC-WHS-REFRESH-PLANNED-DATA"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/23230026077586.md, .private/research/help-center/articles/360013765880.md, .private/research/help-center/articles/360013730520.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Planning copies the article's catalog data into the planned line; later edits of either side do not propagate to the other until the planner explicitly refreshes selected lines from the catalog.

## Rationale

Quoted conditions must stay stable while project-specific edits remain possible.

## Examples

See SB-WHS-CATALOG-PRICE-CHANGE-NOT-PROPAGATED.

## Exceptions

None.
