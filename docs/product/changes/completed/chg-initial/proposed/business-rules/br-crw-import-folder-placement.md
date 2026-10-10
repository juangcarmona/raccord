---
id: BR-CRW-IMPORT-FOLDER-PLACEMENT
type: business-rule
title: "Imported records are placed under the chosen root folder"
status: draft
applies-to:
  - "UC-CRW-IMPORT-CREW-MEMBERS"
  - "UC-CRW-IMPORT-VEHICLES"
uses-terms:
  - "TERM-RESOURCE-IMPORT"
  - "TERM-RECORD-FOLDER"
provenance:
  source: ".private/research/help-center/articles/360014366299.md, .private/research/help-center/articles/360014307939.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Imported crew members and vehicles are stored under the root folder chosen for the import, at the folder path given in the file's folder column; missing folders are created; without a folder column all rows land directly in the root folder.

## Rationale

Keeps imported data organised.

## Examples

Path freelancers/audio technicians under root Crew creates Crew/freelancers/audio technicians.

## Exceptions

None.
