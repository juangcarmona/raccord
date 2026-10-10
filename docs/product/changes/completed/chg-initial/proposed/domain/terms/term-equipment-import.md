---
id: TERM-EQUIPMENT-IMPORT
type: domain-term
title: "Equipment import"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms:
  - "catalog import"
uses-terms:
  - "TERM-IMPORT-MODE"
  - "TERM-IMPORT-MATCH-COLUMN"
  - "TERM-EQUIPMENT-FOLDER"
  - "TERM-TAG"
  - "TERM-EQUIPMENT-ITEM"
provenance:
  source: ".private/research/help-center/articles/360014141299.md, .private/research/help-center/articles/24338169105298.md, .private/research/help-center/articles/24320695034642.md, .private/research/help-center/articles/13725992768786.md, .private/research/help-center/articles/360014293220.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A run that creates or updates catalog items from an uploaded spreadsheet or CSV file. Each run has an import mode, a header choice, a mapping of file columns to equipment fields (including custom fields), a root folder, optional match columns, a preview of the first 10 rows, and a result listing successful and failed rows with reasons and a downloadable file of failures. Every created item receives a tag named after the import date and hour.

In the file, each item is described on one row, with its quantity.

## Distinguish From

- Project equipment list import: plans lines on one project.
- Content import: fills kit default content.
- Bulk update: an import in update mode based on an export.

## Usage

Initial load of the catalog and later mass changes.
