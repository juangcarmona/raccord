---
id: BR-PRJ-FOLDER-DELETE-CASCADE
type: business-rule
title: "Deleting a file folder deletes its files"
status: active
applies-to:
  - "UC-PRJ-ATTACH-FILE"
  - "UC-PRJ-MANAGE-FILE-FOLDERS"
uses-terms:
  - "TERM-FILE-FOLDER"
provenance:
  source: ".private/research/help-center/articles/360013638440.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Deleting a file folder deletes all files it contains, after confirmation.

## Rationale

Folders own their content.

## Examples

Deleting 'Old drawings' removes its ten files.

## Exceptions

None.
