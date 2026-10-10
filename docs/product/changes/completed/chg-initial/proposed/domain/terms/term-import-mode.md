---
id: TERM-IMPORT-MODE
type: domain-term
title: "Import mode"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms: []
uses-terms:
  - "TERM-EQUIPMENT-IMPORT"
  - "TERM-IMPORT-MATCH-COLUMN"
  - "TERM-DEFAULT-CONTENT"
provenance:
  source: ".private/research/help-center/articles/360014141299.md, .private/research/help-center/articles/360014293220.md, .private/research/help-center/articles/20045646271250.md"
  confidence: high
  recovered-from: documentation
---

## Definition

How an import treats rows that may already exist.

For equipment imports:

| Mode | Effect |
| --- | --- |
| All rows as new | Every row creates a new item. |
| Add new, skip existing | Rows matching an existing item (by match columns) are ignored. |
| Add new and update existing | Matching items get the differing values; others are created. Scan codes in the file either replace existing codes or are added alongside them. |

For kit content imports:

| Mode | Effect |
| --- | --- |
| Update content | Lines in the file are added or updated; existing lines not in the file stay. |
| Overwrite content | The kit's content becomes exactly the file's lines; others are deleted. |

## Distinguish From

- Match column: decides which rows are existing; the mode decides what to do with them.

## Usage

Chosen at the start of each import.
