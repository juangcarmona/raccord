---
id: "FR-DOC-012"
type: functional-requirement
title: "Keep content off audiences it is not meant for"
status: draft
derived-from:
  - "UC-DOC-HIDE-ITEMS-ON-FINANCIAL-DOCUMENTS"
  - "BR-DOC-GROUP-PRICE-VISIBILITY"
  - "BR-DOC-HIDE-ZERO-PRICED"
verification:
  - scenario-ref: "SB-DOC-GROUP-EXCLUDED-FROM-PRICE"
  - scenario-ref: "SB-DOC-FULLY-DISCOUNTED-LINE-HIDDEN"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360017647860.md, .private/research/help-center/articles/360017648140.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST omit groups excluded from price calculation and zero-priced lines from financial documents.

## Rationale

Customers see what concerns them.
