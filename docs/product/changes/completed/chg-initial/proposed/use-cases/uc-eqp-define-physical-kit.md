---
id: UC-EQP-DEFINE-PHYSICAL-KIT
type: use-case
title: "Define a physical kit"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-PHYSICAL-KIT-CONSUMES-CONTENT-STOCK"
  - "BR-EQP-PHYSICAL-KIT-AVAILABILITY"
  - "BR-EQP-KIT-WEIGHT"
  - "BR-EQP-KIT-PRICING"
  - "BR-EQP-DEFAULT-CONTENT-CATALOG-ONLY"
  - "BR-EQP-CONTENT-BINDING-DEFAULT"
uses-terms:
  - "TERM-PHYSICAL-KIT"
  - "TERM-DEFAULT-CONTENT"
  - "TERM-CONTENT-STOCK-BINDING"
  - "TERM-EMPTY-WEIGHT"
provenance:
  source: ".private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/4402870209682.md, .private/research/help-center/articles/360018672100.md, .private/research/help-center/articles/20084575989010.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A case or rack with fixed content is in the catalog and can be planned, scanned and booked as one unit.

## Trigger

The company assembles equipment that always travels together.

## Preconditions

- The content items exist in the catalog.

## Main Flow

1. The equipment manager creates or opens a physical item and allows it to have content.
2. The manager searches catalog items and adds each with the quantity per kit as default content.
3. The manager sets the kit's own price, empty weight and dimensions.
4. The product saves the kit and deducts, for content bound to the kit's stock, kit quantity times line quantity from each content item's free stock.

## Alternative Flows

- 2a. Content is another kit: it becomes a nested kit (UC-EQP-NEST-KITS).
- 2b. A sale item is added as content: the sale item is never bound to the kit's stock.
- 4a. Some content should remain rentable separately: the manager marks those lines as available outside the kit (UC-EQP-CONFIGURE-DEFAULT-CONTENT).

## Failure Conditions

- More content is bound than the content item's stock allows: the product saves it, and the content item shows a permanent shortage.

## Postconditions

- The kit is plannable; its weight, price and stock follow the kit rules.
