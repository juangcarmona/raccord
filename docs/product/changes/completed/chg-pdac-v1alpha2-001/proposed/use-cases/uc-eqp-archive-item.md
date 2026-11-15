---
id: UC-EQP-ARCHIVE-ITEM
type: use-case
title: "Archive, restore or delete equipment"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-ARCHIVE-PRESERVES-HISTORY"
  - "BR-EQP-HIDDEN-ITEM-STILL-BOOKABLE"
uses-terms:
  - "TERM-EQUIPMENT-RECORD-STATE"
  - "TERM-ARCHIVED-EQUIPMENT"
  - "TERM-PLANNER-VISIBILITY"
provenance:
  source: ".private/research/help-center/articles/360013852060.md, .private/research/help-center/articles/360013718240.md, .private/research/help-center/articles/360014141299.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Equipment no longer owned or offered leaves stock and planning without damaging history.

## Trigger

Items are sold, scrapped, replaced, or created by mistake.

## Preconditions

- The items exist.

## Main Flow

1. The equipment manager selects one or more items and archives them.
2. The product removes them from stock and from planning and lists them in the archive.
3. Projects that used them still show them (struck through in planning views, normally on documents and in prices).

## Alternative Flows

- 1a. The item should stay in stock but no longer be offered: the manager turns off planner visibility instead.
- 3a. Restore: the manager restores an archived item; it returns to stock and planning.
- 3b. Delete: the manager permanently deletes an archived item; it cannot be recovered.
- 1b. Undoing an import: the manager selects all items carrying that import's tag and archives them.

## Failure Conditions

- Items hidden by active filters or tags cannot be found; the manager clears filters first.

## Postconditions

- The items are archived, active again, or gone, as chosen.
