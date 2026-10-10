---
id: JRN-EQP-KEEP-STOCK-ACCURATE
type: journey
title: "Keep stock accurate over time"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
steps:
  - use-case: "UC-EQP-SCHEDULE-STOCK-MOVEMENT"
  - use-case: "UC-EQP-COUNT-SELECTED-EQUIPMENT"
  - use-case: "UC-EQP-CORRECT-STOCK"
  - use-case: "UC-EQP-SELL-RENTAL-EQUIPMENT"
  - use-case: "UC-EQP-ARCHIVE-ITEM"
provenance:
  source: ".private/research/help-center/articles/16459216697106.md, .private/research/help-center/articles/360015957259.md, .private/research/help-center/articles/360015960120.md, .private/research/help-center/articles/360013877120.md, .private/research/help-center/articles/360013812939.md"
  confidence: high
  recovered-from: documentation
---

## Intended Outcome

Recorded stock matches the physical inventory throughout the year.

## Entry Conditions

- The catalog is set up.

## Journey Narrative

Purchases are entered as scheduled deliveries or stock movements with their dates. Periodic counts, of selected items or of all items, compare shelves with expected stock and correct stock with the differences found; remaining discrepancies are corrected by hand. Sold rental units leave stock through a sale copy, and fully retired items are archived.

## Variants and Branches

- A count of only some items is enough when only part of the warehouse is in doubt.

## Completion Conditions

Stock is reconciled with the physical inventory.
