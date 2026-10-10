---
id: TERM-RECORD-IDENTIFIER
type: domain-term
title: "Record identifier"
status: draft
defined-in: "BC-WORKSPACE"
synonyms: []
uses-terms:
  - "TERM-IMPORT-MATCH-COLUMN"
  - "TERM-NUMBER-SERIES"
provenance:
  source: ".private/research/help-center/articles/36084108749842.md, .private/research/help-center/articles/36812030570770.md, .private/research/help-center/articles/36811784028178.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The internal number the product assigns to every record when it is created. It is unique, never reused, cannot be edited by anyone, and is the reliable key for matching file rows to existing records during imports, unlike names or codes, which can be duplicated or changed.

## Distinguish From

Not a project number, contact number or equipment code, which are user-facing and editable.

## Usage

Included as a column in exports; used as match column for updates by import.
