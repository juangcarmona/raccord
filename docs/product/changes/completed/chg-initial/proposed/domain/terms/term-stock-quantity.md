---
id: TERM-STOCK-QUANTITY
type: domain-term
title: "Stock quantity"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms:
  - "current quantity"
  - "stock level"
uses-terms:
  - "TERM-STOCK-MOVEMENT"
  - "TERM-CONTENT-STOCK-BINDING"
  - "TERM-EQUIPMENT-AVAILABILITY"
provenance:
  source: ".private/research/help-center/articles/31926136198546.md, .private/research/help-center/articles/360013580119.md, .private/research/help-center/articles/4402870209682.md, .private/research/help-center/articles/360015957259.md, .private/research/help-center/articles/16459216697106.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The number of units of an item the company owns at a point in time.

- For items and tracked sale items it is the result of all stock movements up to that date.
- For a virtual kit it is derived: the minimum, over its rental content lines, of the content item's quantity divided by the line quantity, rounded down.
- Content of physical kits bound to the kit's stock is subtracted from the free stock of the content item (kit quantity times line quantity).

Stock quantity is the basis on which availability over time is computed.

## Distinguish From

- Availability: stock minus what is planned, reserved or otherwise out over a period.
- Expected warehouse stock: what should physically be on the shelves at a moment.

## Usage

Shown per item; changed by movements, imports, counts, loss and found reports.
