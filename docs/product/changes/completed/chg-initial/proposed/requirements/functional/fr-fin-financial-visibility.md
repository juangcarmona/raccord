---
id: FR-FIN-FINANCIAL-VISIBILITY
type: functional-requirement
title: "Hide financial details per role"
status: draft
derived-from:
  - "UC-FIN-RESTRICT-FINANCIAL-VISIBILITY"
  - "BR-FIN-FINANCIAL-VISIBILITY"
verification:
  - scenario-ref: "SB-FIN-HIDDEN-FINANCIALS-STILL-UPDATE"
uses-terms:
  - "TERM-FINANCIAL-DETAILS"
provenance:
  source: ".private/research/help-center/articles/4409044768658.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST hide all financial details from roles without financial visibility while keeping planning possible.

## Rationale

Confidentiality of margins.
