---
id: UC-EQP-CONFIGURE-ACCESSORIES
type: use-case
title: "Link accessories to an item"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-ACCESSORY-OPTIONS"
  - "BR-EQP-ACCESSORY-DETACHED-WHEN-PLANNED"
  - "BR-EQP-ACCESSORIES-NOT-INHERITED"
uses-terms:
  - "TERM-ACCESSORY"
  - "TERM-EQUIPMENT-ITEM"
provenance:
  source: ".private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/4402870209682.md, .private/research/help-center/articles/360017699799.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Items that usually accompany another item are suggested or added automatically when it is planned.

## Trigger

The company wants planners not to forget companion items.

## Preconditions

- Both items exist.

## Main Flow

1. The equipment manager opens an item or kit and its accessories.
2. The manager searches and adds an accessory item.
3. The manager sets automatic, skip if already present, always as new line, and free.
4. The product saves the link.

## Alternative Flows

- 3a. Non-automatic accessory: planners are asked each time whether to add it.

## Failure Conditions

- The manager lacks permission to change items: the link is refused and nothing is saved.
- The accessory is the item itself, is already linked, or no longer exists: it is refused with the reason and nothing is saved.
- Another user changed the same item in the meantime: the link is refused and the manager must reload and retry.
- A kit that contains the item does not acquire its accessories, so none are added when the kit is planned (BR-EQP-ACCESSORIES-NOT-INHERITED).

## Postconditions

- Planning the item triggers the accessory behaviour.
