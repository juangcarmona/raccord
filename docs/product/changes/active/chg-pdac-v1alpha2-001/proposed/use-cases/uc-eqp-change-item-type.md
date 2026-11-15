---
id: UC-EQP-CHANGE-ITEM-TYPE
type: use-case
title: "Change the type of an equipment item"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-TYPE-FIXED-ONCE-PLANNED"
  - "BR-EQP-ARCHIVE-PRESERVES-HISTORY"
uses-terms:
  - "TERM-EQUIPMENT-TYPE"
  - "TERM-EQUIPMENT-RECORD-STATE"
  - "TERM-ARCHIVED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360013718240.md, .private/research/help-center/articles/360013703140.md"
  confidence: high
  recovered-from: documentation
---

## Goal

An item is restructured (single item, physical kit, virtual kit) without corrupting projects that already used it.

## Trigger

The company decides an item should be handled as a different structure.

## Preconditions

- The item exists.

## Main Flow

1. The equipment manager opens the item and chooses another type.
2. The product checks that the item has never been planned on any project or template.
3. The product changes the type and saves.

## Alternative Flows

- 2a. The item has been planned: the product does not allow the change. The manager duplicates the item, changes the type on the copy, saves it and archives the original. From then on projects use the copy, while past projects keep the archived original.

## Failure Conditions

- Attempt to change a planned item's type: refused.

## Postconditions

- Exactly one active item of the intended type exists; historic plans still reference the original.
