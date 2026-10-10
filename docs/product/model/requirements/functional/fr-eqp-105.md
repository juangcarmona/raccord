---
id: FR-EQP-105
type: functional-requirement
title: "List the projects that used selected equipment"
status: draft
derived-from:
  - "UC-EQP-VIEW-EQUIPMENT-PROJECT-HISTORY"
  - "BR-EQP-PROJECT-HISTORY-ALL-SELECTED"
verification:
  - id: "PROJECTS-USING-ITEMS"
    scenario: "Given projects containing light model A only, B only, and both, when the user selects A and B and asks for their projects, only the project containing both is listed."
uses-terms:
  - "TERM-EQUIPMENT-ITEM"
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/17484144899986.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a user select one or more catalog items and display the projects in which they were planned, listing only projects that contain every selected item. The product MUST let the user widen the period or clear filters when active filters hide results.

## Rationale

Questions such as which jobs used a given model are the starting point of any quality or fault investigation.
