---
id: BR-FIN-CREDIT-NOTE-CORRECTION
type: business-rule
title: "Finalized invoices are corrected with credit notes"
status: draft
applies-to:
  - "UC-FIN-CREATE-CREDIT-NOTE"
uses-terms:
  - "TERM-CREDIT-NOTE"
  - "TERM-ISSUED-INVOICE"
provenance:
  source: ".private/research/help-center/articles/360017696859.md, .private/research/help-center/articles/6819935599378.md, .private/research/help-center/articles/18560784878610.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A wrong finalized invoice is corrected by creating a credit note from it, which is a new document that deducts the original (fully or partially) and gives a new total; the original remains unchanged.

## Rationale

Issued documents must stay intact for legal reasons.

## Examples

A credit note for 300.00 on an invoice of 1,000.00 leaves the original unchanged and reduces what is owed to 700.00.

## Exceptions

A freestanding refund needs no original invoice.
