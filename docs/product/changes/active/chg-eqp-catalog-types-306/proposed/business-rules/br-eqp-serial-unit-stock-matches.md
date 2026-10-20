---
id: BR-EQP-SERIAL-UNIT-STOCK-MATCHES
type: business-rule
title: "Serialized unit records agree with catalog quantity"
status: draft
applies-to:
    - "UC-EQP-CREATE-ITEM"
    - "UC-EQP-CREATE-SALE-ITEM"
uses-terms:
    - "TERM-SERIAL-NUMBERED-UNIT"
    - "TERM-STOCK-QUANTITY"
provenance:
    source: "Issue #306"
    confidence: medium
    recovered-from: inference
---

## Rule

When an item is configured for serial tracking, each represented physical unit has one serial identifier, one unit contributes exactly one to its catalog quantity, and changes to the unit records and aggregate quantity are committed together. An item configured for aggregate-only tracking has no serial-unit records.

## Rationale

Individually tracked units and aggregate stock must not disagree, since Projects and Warehouse use the item quantity to plan and move equipment.

## Examples

- A serial-tracked camera item with four registered units has catalog quantity four.
- A correction that removes a registered unit and reduces stock updates both records in one operation.
- A quantity-only item has its stock quantity without individual serial-unit records.

## Exceptions

The treatment of booked, sold, lost and corrected units, and the rules for switching tracking modes on an existing item, remain subject to the open questions in `CHG-EQP-CATALOG-TYPES-306`.
