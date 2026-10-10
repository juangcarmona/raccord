---
id: TERM-IMPORT-MATCH-COLUMN
type: domain-term
title: "Import match column"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms: []
uses-terms:
  - "TERM-EQUIPMENT-IMPORT"
  - "TERM-EQUIPMENT-CODE"
provenance:
  source: ".private/research/help-center/articles/360014141299.md, .private/research/help-center/articles/360014293220.md"
  confidence: high
  recovered-from: documentation
---

## Definition

One or more columns whose values identify which existing item a file row refers to (record identifier, code or name). Values in match columns must be unique in the file and in the catalog; a match column must never be a column being changed. The exported record identifiers are the recommended match because they are inherently unique.

## Distinguish From

- Mapped column: any column assigned to a field; only match columns identify records.

## Usage

Required for skip-existing and update modes.
