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
uses-terms:
  - "TERM-EQUIPMENT-ITEM"
  - "TERM-EQUIPMENT-TYPE"
  - "TERM-EQUIPMENT-USAGE"
  - "TERM-RENTAL-PRICE"
provenance:
  source: ".private/research/help-center/articles/31926136198546.md, .private/research/help-center/articles/360013580119.md, .private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/360013086279.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A new catalog item exists with the structure and usage that match how the company handles it, ready to be planned.

## Trigger

The company acquires or starts offering a kind of equipment that is not yet in the catalog.

## Preconditions

- The equipment manager may maintain the catalog.

## Main Flow

1. The equipment manager starts a new item.
2. The manager enters the name, the rental price per day, and the quantity owned.
3. The manager states whether the item is rented or sold.
4. The manager states whether it is a single item or a kit, and for a kit whether it is physical or virtual.
5. The manager optionally chooses an image.
6. The product saves the item, generating an equipment code.
7. The product shows the item details, where advanced data and content can be completed.

## Alternative Flows

- 3a. Sale: the product asks for a sale price and stock management mode instead of rental price (see UC-EQP-CREATE-SALE-ITEM).
- 4a. Virtual kit: quantity is not asked because stock is derived from content.
- 6a. The manager chose to create another: after saving, a blank creation form opens again.
- 1a. Many items at once: the manager imports a file instead (UC-EQP-IMPORT-EQUIPMENT).

## Failure Conditions

- Name missing: the item is not saved.
- Name missing, or a quantity entered for a virtual kit: not accepted.

## Postconditions

- The item is active in the catalog, in its folder, with its stock.
- Settings can be adjusted later, except the type once the item has been planned.
