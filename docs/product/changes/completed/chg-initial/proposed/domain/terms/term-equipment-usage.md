---
id: TERM-EQUIPMENT-USAGE
type: domain-term
title: "Equipment usage"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms:
  - "rental or sale"
uses-terms:
  - "TERM-RENTAL-ITEM"
  - "TERM-SALE-ITEM"
provenance:
  source: ".private/research/help-center/articles/31926136198546.md, .private/research/help-center/articles/360013580119.md, .private/research/help-center/articles/13021031495698.md, .private/research/help-center/articles/360013812939.md"
  confidence: high
  recovered-from: documentation
---

## Definition

Whether an equipment item is offered for rent or for sale. Exactly one applies:

| Usage | Meaning |
| --- | --- |
| Rental | Goes out to projects and comes back to stock afterwards (microphones, lights, cases). |
| Sale | Is sold or consumed and does not come back (tape, batteries, haze fluid). |

Usage decides which price fields apply (rental price and list price, or sale price and purchase price), which stock options exist (stock management mode and critical stock for sale items).

## Distinguish From

- Equipment type: structure of the item, independent of usage.
- Selling a rental item: done by creating a separate sale item, not by flipping the usage of a planned rental item.

## Usage

Chosen at creation and in imports; drives price fields, stock behaviour and warehouse effects.
