---
id: FR-EQP-011
type: functional-requirement
title: "Archive without losing history"
status: draft
derived-from:
  - "UC-EQP-ARCHIVE-ITEM"
  - "BR-EQP-ARCHIVE-PRESERVES-HISTORY"
  - "BR-EQP-HIDDEN-ITEM-STILL-BOOKABLE"
verification:
  - scenario-ref: "SB-EQP-ARCHIVED-ON-PROJECT"
  - scenario-ref: "SB-EQP-HIDDEN-BOOKED"
uses-terms:
  - "TERM-EQUIPMENT-RECORD-STATE"
  - "TERM-PLANNER-VISIBILITY"
provenance:
  source: ".private/research/help-center/articles/360013852060.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let users archive, restore and permanently delete equipment items, and hide items from planning, while archived items remain on historical projects and documents.

## Rationale

Retirement without corrupting history.
