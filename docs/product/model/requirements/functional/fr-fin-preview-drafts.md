---
id: FR-FIN-PREVIEW-DRAFTS
type: functional-requirement
title: "Mark non-final invoices as previews"
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

The product MUST mark documents of non-finalized invoices as previews and keep them out of exports.

## Rationale

Prevents provisional invoices circulating.
