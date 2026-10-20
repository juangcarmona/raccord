---
id: FR-EQP-001
type: functional-requirement
title: "Classify every equipment item"
status: draft
derived-from:
    - "UC-EQP-CREATE-ITEM"
    - "BR-EQP-NEW-ITEM-IS-SINGLE"
    - "BR-EQP-SERIAL-UNIT-STOCK-MATCHES"
verification:
    - scenario: "An item created with sale usage, virtual kit type is stored with exactly those classifications."
    - scenario: "A serial-tracked item stores one registered unit per unit of stock quantity."
uses-terms:
    - "TERM-EQUIPMENT-TYPE"
    - "TERM-EQUIPMENT-USAGE"
    - "TERM-SERIAL-NUMBERED-UNIT"
provenance:
    source: ".private/research/help-center/articles/31926136198546.md, .private/research/help-center/articles/360013580119.md; issue #306"
    confidence: high
    recovered-from: documentation
---

## Requirement

The product MUST record for every equipment item exactly one type (single item, physical kit, virtual kit) and exactly one usage (rental, sale). For physical stock configured for serial tracking, the product MUST represent each unit individually and keep the number of registered units equal to the item's stock quantity.

## Rationale

These classifications determine planning, stock, warehouse and pricing behavior. Serial records make individual physical units traceable while retaining a consistent aggregate quantity.
