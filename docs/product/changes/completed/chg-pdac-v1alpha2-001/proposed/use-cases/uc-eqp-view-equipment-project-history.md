---
id: UC-EQP-VIEW-EQUIPMENT-PROJECT-HISTORY
type: use-case
title: "List projects that used given equipment"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-EQUIPMENT-MANAGER"
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-PROJECT-HISTORY-ALL-SELECTED"
uses-terms:
  - "TERM-EQUIPMENT-ITEM"
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/17484144899986.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The user sees on which projects one or more catalog items were planned.

## Trigger

A question such as which jobs used a light model, or where a problem may have started.

## Preconditions

- The items exist in the catalog.

## Main Flow

1. The user selects one or more items in the catalog.
2. The user asks to display their projects.
3. The product opens the project list filtered to projects that contain every selected item.

## Alternative Flows

None.

## Failure Conditions

- Filters left active in the project list may hide results; the user widens the period or clears filters.

## Postconditions

- The relevant projects are listed.
