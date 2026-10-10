---
id: TERM-STOCK-MOVEMENT
type: domain-term
title: "Stock movement"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms:
  - "stock mutation"
uses-terms:
  - "TERM-STOCK-QUANTITY"
  - "TERM-SCHEDULED-STOCK-MOVEMENT"
  - "TERM-STOCK-CORRECTION"
provenance:
  source: ".private/research/help-center/articles/16459216697106.md, .private/research/help-center/articles/360015957259.md, .private/research/help-center/articles/360013812939.md, .private/research/help-center/articles/360015960120.md, .private/research/help-center/articles/12814405010834.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A dated change to the stock of an item or tracked sale item, with a quantity (positive or negative), a description and optional details. Kinds:

| Kind | Origin |
| --- | --- |
| Delivery | Stock added from a date, possibly in the future. |
| Reduction | Stock removed from a date (negative delivery). |
| Correction | Stock reset to a stated new level. |
| Import | Quantity set or changed through an equipment import or bulk update. |
| Count adjustment | Difference applied when an inventory count is completed. |
| Loss and found | Stock removed when units are reported lost and restored when found. |

## Distinguish From

- Warehouse booking: moves planned equipment to and from projects without changing owned stock (except for sale items).

## Usage

Listed as the stock history of an item; drives stock quantity over time and appears on the availability timeline as stock increases and decreases.
