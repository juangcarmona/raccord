---
id: BR-EQP-CONTENT-IMPORT-MODES
type: business-rule
title: "Content import update versus overwrite"
status: draft
applies-to:
  - "UC-EQP-IMPORT-KIT-CONTENT"
uses-terms:
  - "TERM-IMPORT-MODE"
  - "TERM-DEFAULT-CONTENT"
provenance:
  source: ".private/research/help-center/articles/20045646271250.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A content import in update mode adds and updates the default content lines in the file while leaving other lines of those kits untouched; in overwrite mode the kits' content becomes exactly the file's lines and other lines are deleted.

## Rationale

Supports both incremental and authoritative loads.

## Examples

- A kit with lines A and B and a file with B and C: update yields A, B, C; overwrite yields B, C.

## Exceptions

None.
