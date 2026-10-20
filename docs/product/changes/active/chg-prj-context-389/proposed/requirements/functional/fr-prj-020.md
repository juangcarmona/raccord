---
id: FR-PRJ-020
type: functional-requirement
title: "Attach files in folders"
status: active
derived-from:
  - "UC-PRJ-ATTACH-FILE"
  - "UC-PRJ-MANAGE-FILE-FOLDERS"
  - "BR-PRJ-CONFIDENTIAL-FOLDER"
  - "BR-PRJ-FOLDER-DELETE-CASCADE"
  - "BR-PRJ-FILE-RIGHTS"
verification:
  - scenario-ref: "SB-PRJ-CONFIDENTIAL-FOLDER-HIDDEN"
  - scenario-ref: "SB-PRJ-PROJECT-CONTEXT-AND-HISTORY"
  - scenario: "Deleting a folder deletes the files it contains"
uses-terms:
  - "TERM-ATTACHED-FILE"
  - "TERM-FILE-FOLDER"
provenance:
  source: ".private/research/help-center/articles/360013638440.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let authorised users upload, download, organise in nested and confidential folders, move and delete files on records.

## Rationale

Crew needs riders and maps on location.
