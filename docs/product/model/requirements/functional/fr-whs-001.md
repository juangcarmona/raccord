---
id: FR-WHS-001
type: functional-requirement
title: "Plan articles with quantities on projects"
status: draft
derived-from:
  - "UC-WHS-PLAN-EQUIPMENT"
  - "BR-WHS-PLANNED-SNAPSHOT"
verification:
  - scenario-ref: "SB-WHS-CATALOG-PRICE-CHANGE-NOT-PROPAGATED"
  - scenario: "Planning 3 units of an article creates one line of 3 in the chosen group with the catalog's current price"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360013730520.md, .private/research/help-center/articles/23230026077586.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let planners add catalog articles, combinations and accessories to a project's equipment groups with quantities, by search, tags, folders, multi-selection, typed quantity or keyboard-only entry, storing each as an independent snapshot of catalog data.

## Rationale

Planning is the root of availability, packing and pricing.
