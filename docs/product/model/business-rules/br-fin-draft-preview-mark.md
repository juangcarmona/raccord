---
id: BR-FIN-DRAFT-PREVIEW-MARK
type: business-rule
title: "Non-final invoices carry a preview mark and are not exportable"
status: draft
applies-to: []
uses-terms:
  - "TERM-INVOICE-PREVIEW"
  - "TERM-INVOICE-STATUS"
provenance:
  source: ".private/research/help-center/articles/6819935599378.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An open invoice draft is editable, duplicable and discardable. Its document carries a preview mark and cannot be exported without the mark. Issuing regenerates the document without the mark and enables e-mailing, download and export. A removed draft is neither editable nor exportable.

## Rationale

Prevents preliminary documents being taken for real invoices.

## Examples

See SB-FIN-PREVIEW-NOT-EXPORTED.

## Exceptions

None.
