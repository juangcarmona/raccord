---
id: TERM-EQUIPMENT-RECORD-STATE
type: domain-term
title: "Equipment record state"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms: []
uses-terms:
  - "TERM-EQUIPMENT-ITEM"
  - "TERM-ARCHIVED-EQUIPMENT"
  - "TERM-PLANNER-VISIBILITY"
provenance:
  source: ".private/research/help-center/articles/360013852060.md, .private/research/help-center/articles/360013718240.md, .private/research/help-center/articles/360014141299.md, .private/research/help-center/articles/360013812939.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The lifecycle of an equipment item record in the catalog.

States:

| State | Meaning |
| --- | --- |
| Active | In stock and plannable (subject to planner visibility). |
| Archived | Taken completely out of stock and no longer plannable, but kept so that past projects, documents and statistics stay intact. Shown struck through on projects where it was planned; shown normally on documents and in price calculations. |
| Deleted | Permanently removed; cannot be recovered. |

Transitions:

| From | To | Action |
| --- | --- | --- |
| Active | Archived | Archive (single or bulk selection, including all items of one import tag). |
| Archived | Active | Restore. |
| Archived | Deleted | Permanent delete (discouraged because it can damage projects and documents that used the item). |

## Distinguish From

- Planner visibility: an active item can be hidden from planning while staying in stock.

## Usage

Used to retire items, to replace an item whose type must change, to undo a failed import, and after selling all units of a rental item.
