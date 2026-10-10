---
id: BR-FIN-MOMENT-PERCENTAGES
type: business-rule
title: "Invoice moment parts share the project total"
status: draft
applies-to:
  - "UC-FIN-CONFIGURE-INVOICE-MOMENTS"
  - "UC-FIN-GENERATE-PROJECT-INVOICE"
uses-terms:
  - "TERM-INVOICE-MOMENT"
provenance:
  source: ".private/research/help-center/articles/360013628279.md, .private/research/help-center/articles/360017647300.md, .private/research/help-center/articles/360017700379.md, .private/research/help-center/articles/360015935839.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

Each part of an invoice moment invoices a percentage of the project total, the parts together covering the whole total, and the percentage of the due part is proposed when generating the invoice.

## Rationale

Staged billing must neither miss nor double-bill part of the price.

## Examples

30% before and 70% after the event.

## Exceptions

None.
