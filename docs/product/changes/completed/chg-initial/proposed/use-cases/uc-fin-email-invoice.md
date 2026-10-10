---
id: UC-FIN-EMAIL-INVOICE
type: use-case
title: "E-mail an invoice"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors:
  - "ACT-CUSTOMER"
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-TERMS-ATTACHMENT"
  - "BR-FIN-E-INVOICE-AUTO-ATTACH"
uses-terms:
  - "TERM-ISSUED-INVOICE"
  - "TERM-E-INVOICE"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/360013938099.md, .private/research/help-center/articles/360014032939.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The customer receives the finalized invoice.

## Trigger

The invoice is finalized.

## Preconditions

The invoice is issued.

## Main Flow

1. The administrator e-mails the invoice from the product.
2. The product attaches the document, the terms if configured and the structured e-invoice if automatic generation is on, and adds the configured blind copy address.

## Alternative Flows

The administrator downloads it as PDF, word-processor file or XML instead.

## Failure Conditions

- E-mailing an invoice that is not issued is refused and nothing is sent.
- Missing or invalid recipient address is refused with the reason and nothing is sent.
- A terms file that is not a PDF cannot be attached, so the message is not sent until corrected (BR-FIN-TERMS-ATTACHMENT).
- If the company's mail account is unreachable, sending fails visibly and can be retried (BR-FIN-E-INVOICE-AUTO-ATTACH).
- An actor without permission to send invoices is refused and nothing changes.

## Postconditions

The invoice is sent.
