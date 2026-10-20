---
id: UC-PRJ-ATTACH-FILE
type: use-case
title: "Attach and retrieve files"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-FILE-RIGHTS"
  - "BR-PRJ-CONFIDENTIAL-FOLDER"
  - "BR-PRJ-GENERATED-DOCUMENT-FOLDER"
  - "BR-PRJ-FOLDER-DELETE-CASCADE"
uses-terms:
  - "TERM-ATTACHED-FILE"
  - "TERM-FILE-FOLDER"
provenance:
  source: ".private/research/help-center/articles/360013638440.md, .private/research/help-center/articles/360013838560.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Files needed for a job are stored with it and available to crew.

## Trigger

Maps, riders, drawings or generated documents must be shared.

## Preconditions

The user has rights to add or download files.

## Main Flow

1. The user adds a file to a record, optionally into a folder.
2. Others with download rights download it later, including on location.
3. The user may move a file to another folder or delete it.

## Alternative Flows

- A generated document saved to the project is listed in its files.

## Failure Conditions

Disk space nearly exhausted: the product notifies; files must be deleted or space extended.

## Postconditions

The file is attached.
