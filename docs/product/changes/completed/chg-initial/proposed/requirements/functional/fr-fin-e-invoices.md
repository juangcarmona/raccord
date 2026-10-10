---
id: FR-FIN-E-INVOICES
type: functional-requirement
title: "Produce structured e-invoices"
status: draft
derived-from:
  - "UC-FIN-CONFIGURE-E-INVOICING"
  - "UC-FIN-EXPORT-E-INVOICE"
  - "BR-FIN-E-INVOICE-IDENTIFIERS"
  - "BR-FIN-E-INVOICE-AUTO-ATTACH"
  - "BR-FIN-TAX-CATEGORY-PER-RATE"
  - "CON-FIN-E-INVOICE-STRUCTURED"
verification:
  - scenario: "With automatic generation on, an e-mailed invoice carries the structured XML file; an invoice lacking customer identification number is flagged."
uses-terms:
  - "TERM-E-INVOICE"
provenance:
  source: ".private/research/help-center/articles/360014032939.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST export every finalized invoice in the supported structured format and optionally attach it automatically.

## Rationale

E-invoicing between businesses is mandatory in Spain.
