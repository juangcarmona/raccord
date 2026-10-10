---
id: BR-FIN-TERMS-ATTACHMENT
type: business-rule
title: "Terms and conditions travel with documents per document type"
status: draft
applies-to:
  - "UC-FIN-SEND-QUOTE"
  - "UC-FIN-EMAIL-INVOICE"
  - "UC-FIN-CONFIGURE-FINANCIAL-SETTINGS"
uses-terms:
  - "TERM-TERMS-AND-CONDITIONS"
provenance:
  source: ".private/research/help-center/articles/360013938099.md, .private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The terms-and-conditions file is attached to e-mails of quotes, contracts and invoices according to a separate yes/no setting per document type, and is always shown to customers who sign digitally; it must be a PDF.

## Rationale

Legal conditions must be available when the customer commits.

## Examples

With sending enabled for quotes only, invoice e-mails carry no terms file.

## Exceptions

None.
