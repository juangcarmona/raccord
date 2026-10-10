---
id: TERM-EQUIPMENT-CODE
type: domain-term
title: "Equipment code"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms:
  - "item code"
  - "article code"
uses-terms:
  - "TERM-EQUIPMENT-ITEM"
provenance:
  source: ".private/research/help-center/articles/13141562920338.md, .private/research/help-center/articles/24320695034642.md, .private/research/help-center/articles/360014141299.md, .private/research/help-center/articles/360014293220.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A short identifier of a catalog item, printed on packing slips and usable as the key to match rows during imports and bulk updates. If left empty at creation, the product generates one.

## Distinguish From

- Scan code: the machine-readable label on the item.
- Record identifier exported for bulk updates: system-assigned, never edited.

## Usage

Shown on warehouse documents; recommended match column when updating prices or other data by import.
