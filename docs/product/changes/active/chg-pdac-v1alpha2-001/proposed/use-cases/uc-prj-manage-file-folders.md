---
id: UC-PRJ-MANAGE-FILE-FOLDERS
type: use-case
title: "Organise files in folders"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-CONFIDENTIAL-FOLDER"
  - "BR-PRJ-FOLDER-DELETE-CASCADE"
uses-terms:
  - "TERM-FILE-FOLDER"
provenance:
  source: ".private/research/help-center/articles/360013638440.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Large file sets stay navigable.

## Trigger

A project accumulates many files.

## Preconditions

The user has file rights.

## Main Flow

1. The user creates a folder, optionally inside a parent and optionally confidential.
2. The product stores it; confidential folders are visible only to users who may download all files.

## Alternative Flows

None.

## Failure Conditions

Deleting a folder deletes its files.

## Postconditions

Folder exists.
