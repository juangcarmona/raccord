---
id: BR-EQP-CODE-GENERATED
type: business-rule
title: "Equipment code is generated when left empty"
status: draft
applies-to:
  - "UC-EQP-CREATE-ITEM"
  - "UC-EQP-CONFIGURE-ITEM-DETAILS"
uses-terms:
  - "TERM-EQUIPMENT-CODE"
provenance:
  source: ".private/research/help-center/articles/13141562920338.md"
  confidence: high
  recovered-from: documentation
---

## Rule

If no equipment code is entered for an item, the product assigns one automatically; the code appears on packing slips.

## Rationale

Every item needs a short identifier for warehouse documents and imports.

## Examples

- An item saved with an empty code receives a generated code visible on its next packing slip.

## Exceptions

None.
