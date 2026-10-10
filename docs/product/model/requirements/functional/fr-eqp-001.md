---
id: FR-EQP-001
type: functional-requirement
title: "Classify every equipment item"
status: draft
derived-from:
  - "UC-EQP-CREATE-ITEM"
  - "BR-EQP-NEW-ITEM-IS-SINGLE"
verification:
  - scenario: "An item created with sale usage, virtual kit type is stored with exactly those classifications."
uses-terms:
  - "TERM-EQUIPMENT-TYPE"
  - "TERM-EQUIPMENT-USAGE"
provenance:
  source: ".private/research/help-center/articles/31926136198546.md, .private/research/help-center/articles/360013580119.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST record for every equipment item exactly one type (single item, physical kit, virtual kit) and exactly one usage (rental, sale).

## Rationale

These classifications determine planning, stock, warehouse and pricing behaviour.
