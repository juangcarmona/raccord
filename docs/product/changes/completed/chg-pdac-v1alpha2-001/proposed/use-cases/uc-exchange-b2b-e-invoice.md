---
id: UC-EXCHANGE-B2B-E-INVOICE
type: use-case
title: "Send a B2B e-invoice and track its status"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors:
  - "ACT-B2B-E-INVOICE-PLATFORM"
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-B2B-STATUS-REPORTING"
uses-terms:
  - "TERM-ISSUED-INVOICE"
  - "TERM-COMPANY-CERTIFICATE"
---

## Goal

A business customer receives the issued invoice as a structured e-invoice and its status is known.

## Trigger

An invoice to a Spanish business customer is issued.

## Preconditions

The customer has a Spanish tax identifier; a valid company certificate is installed.

## Main Flow

1. The product produces the structured e-invoice from the issued representation.
2. The product sends it through the Spanish tax authority's public e-invoice solution. Private-platform exchange is outside the first release but remains an adapter boundary for a later Product Change.
3. The platform reports acceptance or rejection and, later, full payment and its date.
4. The product records each status on the invoice.

## Alternative Flows

- An e-invoice addressed to the company is received; the billing administrator accepts or rejects it and later reports its full payment.

## Failure Conditions

- The platform is unreachable: the e-invoice stays pending and is retried.
- The customer rejects the invoice: it stays issued and must be corrected with a rectifying invoice.

## Postconditions

The invoice carries its e-invoice status history.
