---
id: CON-CON-IMPORT-FILE-FORMAT
type: constraint
title: "Contact import file format"
status: active
applies-to:
  - "UC-CON-IMPORT-CONTACTS"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360013553000.md"
  confidence: high
  recovered-from: documentation
---

## Constraint

Contact imports accept a comma-separated values (CSV) file or a single-sheet Excel Open XML (XLSX) spreadsheet with at most 64 columns, plain unformatted text values, no embedded images, and an optional header row. The preview shows at most the first 10 data rows. The product does not promise a maximum row count in this change.

## Rationale

Supported inputs are bounded and predictable while the product has not accepted a row-count limit.

## Consequences

Files with multiple sheets, formatting, images or more than 64 columns must be prepared before import. Users review the preview and validation feedback before confirming creation.
