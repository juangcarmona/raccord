---
id: BR-CRW-IMPORT-MATCH-COLUMN
type: business-rule
title: "Update and skip imports need a unique match column"
status: draft
applies-to:
  - "UC-CRW-IMPORT-CREW-MEMBERS"
  - "UC-CRW-IMPORT-VEHICLES"
uses-terms:
  - "TERM-RESOURCE-IMPORT"
provenance:
  source: ".private/research/help-center/articles/360014366299.md, .private/research/help-center/articles/360014307939.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

In update mode and add-new-skip-existing mode, the planner selects match columns that identify existing records; their values must be unique in the file and the match column must not be a column being updated.

## Rationale

Matching decides which record a row belongs to.

## Examples

Updating phone numbers matched on email works; matching on the phone column itself is not allowed.

## Exceptions

None.
