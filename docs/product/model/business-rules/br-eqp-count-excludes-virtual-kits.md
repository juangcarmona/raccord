---
id: BR-EQP-COUNT-EXCLUDES-VIRTUAL-KITS
type: business-rule
title: "Virtual kits are not counted"
status: draft
applies-to:
  - "UC-EQP-COUNT-SELECTED-EQUIPMENT"
uses-terms:
  - "TERM-VIRTUAL-KIT"
  - "TERM-INVENTORY-COUNT"
provenance:
  source: ".private/research/help-center/articles/360015960120.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Virtual kits are excluded from inventory counts; their content items are counted instead.

## Rationale

A virtual kit has no physical stock of its own.

## Examples

- Selecting a speaker set kit for counting adds no count line for it.

## Exceptions

None.
