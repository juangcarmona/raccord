---
id: UC-EQP-CREATE-ITEM
type: use-case
title: "Create an equipment item"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
    - "BR-EQP-NEW-ITEM-IS-SINGLE"
    - "BR-EQP-CODE-GENERATED"
    - "BR-EQP-SERIAL-UNIT-STOCK-MATCHES"
uses-terms:
    - "TERM-EQUIPMENT-ITEM"
    - "TERM-EQUIPMENT-TYPE"
    - "TERM-EQUIPMENT-USAGE"
    - "TERM-RENTAL-PRICE"
    - "TERM-SERIAL-NUMBERED-UNIT"
provenance:
    source: ".private/research/help-center/articles/31926136198546.md, .private/research/help-center/articles/360013580119.md, .private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/360013086279.md; issue #306"
    confidence: high
    recovered-from: documentation
---

## Goal

A new catalog item exists with its structure, rental usage and stock representation ready for planning.

## Trigger

The company acquires or starts offering a kind of equipment that is not yet in the catalog.

## Preconditions

- The equipment manager may maintain the catalog.

## Main Flow

1. The equipment manager starts a new item.
2. The manager enters its name, rental price per day and quantity owned.
3. The manager marks the item as rental usage and chooses its structural type: single item, physical kit or virtual kit.
4. For a sale item, the manager follows `UC-EQP-CREATE-SALE-ITEM` and enters the sale-specific prices and stock mode.
5. The manager chooses whether physical stock is tracked as individually serial-numbered units.
6. If serial tracking is selected, the manager registers the unit identifiers and the product keeps them consistent with the item quantity.
7. The manager optionally chooses an image.
8. The product saves the item, generating an equipment code.
9. The product shows the item details, where advanced data and content can be completed.

## Alternative Flows

- 3a. Virtual kit: quantity is not asked because stock is derived from content.
- 6a. Serial tracking is not selected: the item is tracked by aggregate quantity.
- 8a. The manager chose to create another: after saving, a blank creation form opens again.
- 1a. Many items at once: the manager imports a file instead (`UC-EQP-IMPORT-EQUIPMENT`).

## Failure Conditions

- Name missing: the item is not saved.
- Name missing, or a quantity entered for a virtual kit: not accepted.
- Serial tracking selected without valid unique unit identifiers or with a quantity that does not match the registered units: not accepted.

## Postconditions

- The item is active in the catalog with its folder and stock representation.
- Settings can be adjusted later, except structural type once the item has been planned.
