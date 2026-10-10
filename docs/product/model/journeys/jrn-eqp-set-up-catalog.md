---
id: JRN-EQP-SET-UP-CATALOG
type: journey
title: "Set up the equipment catalog"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
steps:
  - use-case: "UC-EQP-IMPORT-EQUIPMENT"
  - use-case: "UC-EQP-CONFIGURE-ITEM-DETAILS"
  - use-case: "UC-EQP-DEFINE-PHYSICAL-KIT"
  - use-case: "UC-EQP-DEFINE-VIRTUAL-KIT"
  - use-case: "UC-EQP-IMPORT-KIT-CONTENT"
  - use-case: "UC-EQP-CONFIGURE-ACCESSORIES"
  - use-case: "UC-EQP-SET-ALTERNATIVES"
  - use-case: "UC-EQP-ADD-ITEM-IMAGE"
provenance:
  source: ".private/research/help-center/articles/31926136198546.md, .private/research/help-center/articles/360013580119.md, .private/research/help-center/articles/360014141299.md, .private/research/help-center/articles/20045646271250.md, .private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/360013086279.md"
  confidence: high
  recovered-from: documentation
---

## Intended Outcome

The company's whole inventory is in the catalog, structured and priced, ready for planning and warehouse work.

## Entry Conditions

- A folder structure is decided.
- An inventory list exists, or items will be entered by hand.

## Journey Narrative

The equipment manager imports items from files (or creates them one by one), completing missing data later through bulk updates or item editing. Cases are defined as physical kits and planning sets as virtual kits; their content is attached by hand or through a content import. Accessories and alternatives are linked, and images are added.

## Variants and Branches

- Small inventories skip imports and use item creation.
- A failed import is undone by archiving all items with its tag.

## Completion Conditions

Every owned item exists with correct type, usage, stock and prices; kits have content.
