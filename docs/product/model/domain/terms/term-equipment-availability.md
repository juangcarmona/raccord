---
id: TERM-EQUIPMENT-AVAILABILITY
type: domain-term
title: "Equipment availability"
status: draft
defined-in: BC-EQUIPMENT
synonyms:
  - "availability"
uses-terms:
  - "TERM-AVAILABILITY-FORECAST"
  - "TERM-RESERVABLE-STOCK"
  - "TERM-PLANNING-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/360012699859.md, .private/research/help-center/articles/13369374381714.md, .private/research/help-center/articles/360021519799.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The quantity of an article that is free over a given interval. It is derived from: owned stock over time (including scheduled stock increases and decreases), minus quantities consumed by planned lines in their planning periods according to the effect of their project status, minus lost quantities, adjusted by warehouse movements (early returns release stock, delayed equipment extends the hold). It is expressed in two readings: the availability forecast and the reservable stock.

## Distinguish From

Not the stock level (owned quantity, regardless of commitments).

## Usage

Shown in quick availability checks and the availability timeline; drives shortages.
