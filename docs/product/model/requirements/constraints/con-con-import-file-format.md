---
id: CON-CON-IMPORT-FILE-FORMAT
type: constraint
title: Contact import file format
status: active
applies-to:
  - UC-CON-IMPORT-CONTACTS
uses-terms: []
provenance:
  source: .private/research/help-center/articles/360013553000.md
  confidence: high
  recovered-from: documentation
---

## Constraint

UC-CON-IMPORT-CONTACTS accepts CSV or a single-worksheet XLSX with at most 64 columns, plain-text contact values and an optional header. The preview shows the first 10 data rows. Files larger than 10 MiB, malformed or unsupported workbooks, formulas, embedded images and unsupported cell types are rejected before writes. No maximum row count is prescribed.

## Rationale

Supported inputs are bounded and predictable while the product has not accepted a row-count limit.

## Consequences

Users prepare supported plain-text inputs and review mapping and row validation before confirming. The bounded upload is transient; the product does not preserve it as audit data.
