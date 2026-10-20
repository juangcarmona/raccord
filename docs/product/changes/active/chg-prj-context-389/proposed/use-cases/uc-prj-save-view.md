---
id: UC-PRJ-SAVE-VIEW
type: use-case
title: "Save an overview view"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-SAVED-VIEW-CONTENT"
uses-terms:
  - "TERM-SAVED-VIEW"
provenance:
  source: ".private/research/help-center/articles/26916975812370.md, .private/research/help-center/articles/360012581679.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A useful combination of overview settings is reused in one step.

## Trigger

The same selection is needed repeatedly.

## Preconditions

An overview is open.

## Main Flow

1. The user names a view combining date, tags, status, filters and columns and sets its visibility.
2. The product saves it; earlier presets and column sets can be merged into a view.

## Alternative Flows

None.

## Failure Conditions

- A view without a name, or whose name is already in use, is refused with the reason and nothing is saved.
- An actor without permission to share views cannot give a view wider visibility; it is refused (BR-PRJ-SAVED-VIEW-CONTENT).
- If another user changed the same view in the meantime, the change is refused and the user must reload and retry.

## Postconditions

View available.
