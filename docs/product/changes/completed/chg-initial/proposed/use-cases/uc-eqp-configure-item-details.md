---
id: UC-EQP-CONFIGURE-ITEM-DETAILS
type: use-case
title: "Configure advanced equipment data"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-CODE-GENERATED"
  - "BR-EQP-MARGIN-INFORMATIONAL"
  - "BR-EQP-KIT-WEIGHT"
  - "BR-EQP-TRANSPORT-VOLUME-PACKED-PER"
  - "BR-EQP-SURFACE-PRICING"
  - "BR-EQP-HIDDEN-ITEM-STILL-BOOKABLE"
  - "BR-EQP-REMARK-AUDIENCE"
  - "BR-WHS-KIT-CONTENT-EDITABILITY"
uses-terms:
  - "TERM-EQUIPMENT-ITEM"
  - "TERM-EQUIPMENT-CODE"
  - "TERM-EQUIPMENT-FOLDER"
  - "TERM-EQUIPMENT-GROUP"
  - "TERM-PLANNER-VISIBILITY"
  - "TERM-LOCATION-IN-WAREHOUSE"
  - "TERM-RENTAL-PRICE"
  - "TERM-MARGIN-PRICE"
  - "TERM-VAT-CLASS"
  - "TERM-LEDGER-ACCOUNT"
  - "TERM-DISCOUNT-GROUP"
  - "TERM-FACTOR-GROUP"
  - "TERM-EMPTY-WEIGHT"
  - "TERM-TRANSPORT-VOLUME"
  - "TERM-PACKED-PER"
  - "TERM-SURFACE-ITEM"
  - "TERM-MEASURING-UNIT"
  - "TERM-EQUIPMENT-REMARKS"
  - "TERM-CUSTOM-FIELD"
  - "TERM-TAG"
provenance:
  source: ".private/research/help-center/articles/13141562920338.md, .private/research/help-center/articles/360013580119.md, .private/research/help-center/articles/4402870209682.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The item carries all commercial, physical, logistic and descriptive data needed for pricing, transport, documents and organisation.

## Trigger

An item has been created, or its data must be completed or changed.

## Preconditions

- The item exists and is active.

## Main Flow

1. The equipment manager opens the item.
2. The manager sets or changes the code, folder, default equipment group, planner visibility and, for virtual kits, whether content may be edited per project.
3. The manager records where the item sits inside the warehouse.
4. The manager sets financial data: rental price, discount group, factor group, margin price, VAT class and ledger account.
5. The manager sets physical data: dimensions, weight or empty weight, transport volume (optionally derived from dimensions), packed-per quantity, surface pricing and measuring unit.
6. The manager enters external and internal remarks, custom field values, tags, tasks, notes and files.
7. The product saves the data and uses it from then on for new planned lines, documents and totals.

## Alternative Flows

- 5a. Surface item: the manager enters a standard size and its price; projects then ask for dimensions when planning.
- 2a. Code left empty: the product generates it.
- 3a. The location in the warehouse may stay empty.

## Failure Conditions

- A value incompatible with the item type (for example empty weight on a single item) is not offered.

## Postconditions

- The item's master data is updated; existing planned lines keep their own snapshot of the earlier data.
