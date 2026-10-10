---
id: BR-EQP-IMPORT-MATCH-UNIQUE
type: business-rule
title: "Match columns must be unique and unchanged"
status: draft
applies-to:
  - "UC-EQP-IMPORT-EQUIPMENT"
  - "UC-EQP-MASS-UPDATE-EQUIPMENT"
uses-terms:
  - "TERM-IMPORT-MATCH-COLUMN"
provenance:
  source: ".private/research/help-center/articles/360014141299.md, .private/research/help-center/articles/360014293220.md"
  confidence: high
  recovered-from: documentation
---

## Rule

In skip-existing and update imports, a file row is treated as an existing item when its values in all chosen match columns equal those of a catalog record; match column values must not repeat in the file or catalog, and a column being updated must not be used as a match column.

## Rationale

Ambiguous keys update the wrong record or create duplicates.

## Examples

- Updating prices matched on code updates each item once.
- Matching on name when two items share a name yields an error or a duplicate.

## Exceptions

None.
