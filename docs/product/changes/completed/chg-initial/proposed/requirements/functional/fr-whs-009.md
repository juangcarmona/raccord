---
id: FR-WHS-009
type: functional-requirement
title: "Refresh planned data"
status: draft
derived-from:
  - "UC-WHS-REFRESH-PLANNED-DATA"
  - "BR-WHS-REFRESH-VIRTUAL-CONTENT"
verification:
  - scenario-ref: "SB-WHS-CATALOG-PRICE-CHANGE-NOT-PROPAGATED"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360013765880.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST refresh selected lines from the catalog on request, optionally realigning virtual combination content and recomputing virtual combination prices from content.

## Rationale

Duplicated projects get current data.
