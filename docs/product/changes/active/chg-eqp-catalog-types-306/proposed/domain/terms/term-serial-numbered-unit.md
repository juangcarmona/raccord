---
id: TERM-SERIAL-NUMBERED-UNIT
type: domain-term
title: "Serial-numbered unit"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms:
    - "serial unit"
uses-terms:
    - "TERM-EQUIPMENT-ITEM"
    - "TERM-STOCK-QUANTITY"
provenance:
    source: "Issue #306"
    confidence: high
    recovered-from: inference
---

## Definition

One individually identified physical unit of an equipment catalog item, represented by a serial identifier when its item is configured for serial tracking.

## Distinguish From

- Catalog quantity: an aggregate count of units, rather than the identity of each physical unit.
- Equipment code: identifies the catalog item; a serial identifier distinguishes one unit of that item from other units.

## Usage

An equipment manager may configure an item to track physical units individually. Each registered serial-numbered unit contributes one to the item's stock quantity.
