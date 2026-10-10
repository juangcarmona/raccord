---
id: TERM-EQUIPMENT-TYPE
type: domain-term
title: "Equipment type"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms:
  - "item type"
uses-terms:
  - "TERM-SINGLE-ITEM"
  - "TERM-PHYSICAL-KIT"
  - "TERM-VIRTUAL-KIT"
  - "TERM-EQUIPMENT-ITEM"
provenance:
  source: ".private/research/help-center/articles/31926136198546.md, .private/research/help-center/articles/360013718240.md, .private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/4402870209682.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The structural classification of an equipment item. Exactly one applies:

| Type | Meaning |
| --- | --- |
| Single item | A standalone physical piece, stored and handled on its own (one speaker model, one cable type). |
| Physical kit | A physical item that has fixed content stored together with it, such as a flight case with its contents or a rack with mounted amplifiers. |
| Virtual kit | A planning grouping of separate items that are often, but not always, rented together; nothing is physically stored together. |

New items default to single item. The type can be changed freely only while the item has never been planned on any project; afterwards it is fixed and a change requires duplicating the item and archiving the original.

## Distinguish From

- Equipment usage (rental or sale) is an independent classification.
- Accessories are a link between items, not a type.

## Usage

Chosen at item creation and in imports (type column); governs content behaviour, stock and availability calculation, weight and volume, pricing on projects, and whether scan codes are possible.
