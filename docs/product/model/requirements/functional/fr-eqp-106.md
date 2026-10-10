---
id: FR-EQP-106
type: functional-requirement
title: "Review where an item's stock is"
status: draft
derived-from:
  - "UC-EQP-VIEW-ITEM-INSIGHTS"
verification:
  - id: "ITEM-STOCK-DISTRIBUTION"
    scenario: "Given an item with units booked on a project, planned on another and lost, when the manager opens its overview, total stock, availability and the counts per state are shown, the projects holding units are listed by status with quantities, and lost equipment is listed."
uses-terms:
  - "TERM-EQUIPMENT-ITEM"
  - "TERM-STOCK-QUANTITY"
  - "TERM-LOST-EQUIPMENT"
  - "TERM-DELAYED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/13141562920338.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST show, in an item's overview, total stock and availability and the counts of units delayed, booked on projects, planned, reserved and lost, MUST list the projects currently holding units grouped by status with quantities, and MUST list the item's lost equipment. The product MUST also give access to the item's inventory counts.

## Rationale

A manager answering where an item is needs the whole distribution at a glance.
