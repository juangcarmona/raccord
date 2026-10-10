---
id: BR-WSP-INVOICE-FORWARD-COPY
type: business-rule
title: "Invoice emails can be copied to an accounting inbox"
status: draft
applies-to:
  - "UC-FIN-EMAIL-INVOICE"
  - "UC-FIN-RUN-ACCOUNTING-EXPORT"
uses-terms:
  - "TERM-ISSUED-INVOICE"
provenance:
  source: ".private/research/help-center/articles/36522554053522.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

The company can define an address that receives a blind copy of every invoice email, so the accountant or a mailbox that collects incoming invoices receives each invoice automatically.

## Rationale

Many accountants collect invoices from a dedicated address; this avoids a separate export step.

## Examples

- Entering the accountant's address makes every emailed invoice arrive there too.

## Exceptions

- Invoices that are never emailed are not forwarded.
