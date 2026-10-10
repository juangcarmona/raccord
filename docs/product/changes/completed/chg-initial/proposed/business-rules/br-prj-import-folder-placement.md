---
id: BR-PRJ-IMPORT-FOLDER-PLACEMENT
type: business-rule
title: "Imported records are placed by folder path"
status: draft
applies-to:
  - "UC-PRJ-IMPORT-CONTACTS"
uses-terms:
  - "TERM-RECORD-FOLDER"
provenance:
  source: ".private/research/help-center/articles/360013553000.md, .private/research/help-center/articles/360013700660.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Imported contacts (and other foldered records) are placed under the chosen root folder at the path given in their folder column, creating missing folders; without a folder column all go into the root folder.

## Rationale

Imports should keep the database organised.

## Examples

Path freelancers/audio technicians creates both folders under the root.

## Exceptions

None.
