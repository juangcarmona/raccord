---
id: UC-EQP-DEFINE-VIRTUAL-KIT
type: use-case
title: "Define a virtual kit"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-VIRTUAL-KIT-STOCK-DERIVED"
  - "BR-EQP-KIT-WEIGHT"
  - "BR-EQP-KIT-PRICING"
  - "BR-EQP-DEFAULT-CONTENT-CATALOG-ONLY"
uses-terms:
  - "TERM-VIRTUAL-KIT"
  - "TERM-DEFAULT-CONTENT"
provenance:
  source: ".private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/4402870209682.md, .private/research/help-center/articles/360017696339.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A frequently combined set of items can be planned in one step while each item stays rentable on its own.

## Trigger

The company often rents certain items together but not always.

## Preconditions

- The content items exist in the catalog.

## Main Flow

1. The equipment manager creates or opens an item and sets it as virtual.
2. The manager adds content items with quantities per kit.
3. The manager sets a fixed kit price or chooses to compute the price from the content.
4. The product saves the kit; its stock and availability are from now on derived from its content.

## Alternative Flows

- 3a. The manager recalculates the kit price from the current content inside the kit record.

## Failure Conditions

- A scan code for a virtual kit: not offered.

## Postconditions

- The kit can be planned; planning it adds its content to the project.
