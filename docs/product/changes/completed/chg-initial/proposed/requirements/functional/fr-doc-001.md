---
id: "FR-DOC-001"
type: functional-requirement
title: "Provide a built-in document template per document type"
status: draft
derived-from:
  - "BR-DOC-TYPE-SCOPES-TEMPLATE"
verification:
  - scenario: "Given the built-in packing slip template, when a user generates an invoice, then the packing slip template is not offered"
uses-terms:
  - "TERM-DOCUMENT-TEMPLATE"
  - "TERM-DOCUMENT-TYPE"
provenance:
  source: ".private/research/help-center/articles/360013503420.md, .private/research/help-center/articles/23413689614226.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST provide a built-in template for each document type, scoped to that type, that carries the company's branding.

## Rationale

Each kind of document needs its own layout, ready to use.
