---
id: TERM-ACCESSORY
type: domain-term
title: "Accessory"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms: []
uses-terms:
  - "TERM-EQUIPMENT-ITEM"
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/4402870209682.md, .private/research/help-center/articles/360017699799.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A link from an equipment item (single item, physical kit or virtual kit) to another item that should be suggested or added whenever the first is planned, such as a rain cover for a moving head. Each link carries options:

| Option | Effect |
| --- | --- |
| Automatic | Yes: added silently with the item. No: the planner is asked whether to add it. |
| Skip if already present | Not added again when already planned as a separate line. |
| Always as new line | Yes: always a separate line. No: quantities of the same accessory merge into one line. |
| Free | Added at no charge instead of its own price. |

Once added to a project, the accessory is an ordinary independent line: the link ends, and stock, availability, weight and volume are computed for it separately. Accessories are defined per item; they are not inherited by kits that contain the item.

## Distinguish From

- Kit content: part of the kit itself.
- Alternative: a substitute, not an addition.

## Usage

Speeds up planning and prevents forgotten items.
