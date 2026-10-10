---
id: BR-EQP-IMPORT-DUPLICATE-CODES
type: business-rule
title: "Imports reject duplicate scan codes"
status: draft
applies-to:
  - "UC-EQP-IMPORT-EQUIPMENT"
uses-terms:
  - "TERM-SCAN-CODE"
provenance:
  source: ".private/research/help-center/articles/13725992768786.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An imported row whose scan code already exists in the catalog is not imported; scan codes longer than 25 characters are truncated, which can itself cause such duplicates.

## Rationale

Identifiers must be unique.

## Examples

- Importing an item with a code already used by another item fails for that row only.

## Exceptions

None.
