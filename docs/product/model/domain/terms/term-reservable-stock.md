---
id: TERM-RESERVABLE-STOCK
type: domain-term
title: "Reservable stock"
status: draft
defined-in: BC-EQUIPMENT
synonyms:
  - "warehouse stock"
uses-terms:
  - "TERM-EQUIPMENT-AVAILABILITY"
  - "TERM-RESERVATION"
provenance:
  source: ".private/research/help-center/articles/13369374381714.md, .private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/360012699859.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The reading of availability that subtracts only reserved quantities: the own stock that is still free to be reserved by some project at a moment. Warehouse movements change it (an early return raises it, booking out reserved stock for another project moves it).

## Distinguish From

Not the forecast: an unreserved plan lowers the forecast but leaves reservable stock unchanged.

## Usage

Second line of the availability timeline; the quantity a reservation may claim.
