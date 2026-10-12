---
id: UC-PRJ-MANAGE-RECORD-FOLDERS
type: use-case
title: "Maintain a record folder structure"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-CONTACTS"
governed-by:
  - "BR-PRJ-FOLDER-DELETE-NONEMPTY"
  - "BR-PRJ-FOLDER-PER-MODULE"
uses-terms:
  - "TERM-RECORD-FOLDER"
provenance:
  source: ".private/research/help-center/articles/360013700660.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Records of a module are stored in a logical hierarchy.

## Trigger

Before adding or importing data.

## Preconditions

The user may manage folders.

## Main Flow

1. The user adds root folders and subfolders, renames, reorders and moves them.
2. The product saves.

## Alternative Flows

- Pinning the folder panel next to the list (display preference).

## Failure Conditions

A folder that still contains records cannot be deleted.

## Postconditions

Folder structure updated.
