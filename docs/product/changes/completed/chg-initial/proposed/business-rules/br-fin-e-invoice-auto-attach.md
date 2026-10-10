---
id: BR-FIN-E-INVOICE-AUTO-ATTACH
type: business-rule
title: "Structured files are attached automatically when enabled"
status: draft
applies-to:
  - "UC-FIN-CONFIGURE-E-INVOICING"
  - "UC-FIN-EMAIL-INVOICE"
uses-terms:
  - "TERM-E-INVOICE"
  - "TERM-E-INVOICE-FORMAT"
provenance:
  source: ".private/research/help-center/articles/360014032939.md"
  confidence: high
  recovered-from: documentation
---

## Rule

With automatic generation enabled, every outgoing invoice e-mail carries the structured e-invoice.

## Rationale

Customers receive compliant invoices without extra steps.

## Examples

An invoice e-mailed with automatic generation on carries the XML file next to the PDF.

## Exceptions

None.
