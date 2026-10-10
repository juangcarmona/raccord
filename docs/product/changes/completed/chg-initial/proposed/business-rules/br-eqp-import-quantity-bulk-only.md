---
id: BR-EQP-IMPORT-QUANTITY-BULK-ONLY
type: business-rule
title: "Quantity column sets stock"
status: draft
applies-to:
  - "UC-EQP-IMPORT-EQUIPMENT"
uses-terms:
  - "TERM-EQUIPMENT-IMPORT"
provenance:
  source: ".private/research/help-center/articles/24338169105298.md, .private/research/help-center/articles/24320695034642.md"
  confidence: high
  recovered-from: documentation
---

## Rule

In an equipment import, the quantity column sets the stock of each item, which is described on a single row.

## Rationale

Stock is counted by quantity.

## Examples

- A cable row with quantity 40 creates stock 40.

## Exceptions

None.
