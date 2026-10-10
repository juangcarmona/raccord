---
id: UC-EQP-SET-ALTERNATIVES
type: use-case
title: "Declare alternatives for an item"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-WHS-ALTERNATIVE-SAVED"
uses-terms:
  - "TERM-EQUIPMENT-ALTERNATIVE"
  - "TERM-EQUIPMENT-ITEM"
provenance:
  source: ".private/research/help-center/articles/12690900170514.md, .private/research/help-center/articles/20084575989010.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Shortages can be solved quickly with known substitutes.

## Trigger

The company identifies interchangeable items.

## Preconditions

- Both items exist.

## Main Flow

1. The equipment manager opens the item's alternatives.
2. The manager adds substitute items from the catalog.
3. The product saves them for use when solving shortages.

## Alternative Flows

- 2a. Alternatives can also be added while solving a shortage, which saves them for reuse.

## Failure Conditions

- The manager lacks permission to change items: the alternatives are refused and nothing is saved.
- An alternative that is the item itself, is already listed, or does not exist in the catalog is refused with the reason.
- Another user changed the item's alternatives in the meantime: the change is refused and the manager must reload and retry.

## Postconditions

- The alternatives are offered whenever the item is short.
