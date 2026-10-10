---
id: BR-FIN-DRAFT-PREVIEW-MARK
type: business-rule
title: "Non-final invoices carry a preview mark and are not exportable"
status: draft
applies-to:
  - "UC-FIN-EDIT-INVOICE-DRAFT"
  - "UC-ISSUE-INVOICE"
  - "UC-FIN-RUN-ACCOUNTING-EXPORT"
uses-terms:
  - "TERM-INVOICE-PREVIEW"
  - "TERM-INVOICE-STATUS"
provenance:
  source: ".private/research/help-center/articles/6819935599378.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An invoice that is not finalized is editable, duplicable and deletable, its document carries a preview mark, and it cannot be exported without the mark; finalizing regenerates the document without the mark and enables e-mailing, download and export.

## Rationale

Prevents preliminary documents being taken for real invoices.

## Examples

See SB-FIN-PREVIEW-NOT-EXPORTED.

## Exceptions

None.
