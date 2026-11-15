---
id: FR-FIN-PREVIEW-DRAFTS
type: functional-requirement
title: "Mark open invoice drafts as previews"
status: draft
derived-from:
  - "UC-FIN-EDIT-INVOICE-DRAFT"
  - "BR-FIN-DRAFT-PREVIEW-MARK"
verification:
  - scenario-ref: "SB-FIN-PREVIEW-NOT-EXPORTED"
uses-terms:
  - "TERM-INVOICE-PREVIEW"
provenance:
  source: ".private/research/help-center/articles/6819935599378.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST mark documents of open invoice drafts as previews and keep them out of exports. Removed drafts are not exportable.

## Rationale

Prevents provisional invoices circulating.
