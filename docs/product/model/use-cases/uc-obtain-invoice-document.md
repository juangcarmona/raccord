---
id: UC-OBTAIN-INVOICE-DOCUMENT
type: use-case
title: "Obtain an invoice document"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-INTEGRITY-VERIFICATION"
  - "BR-AUDITED-ACTIONS"
uses-terms:
  - "TERM-INVOICE-DOCUMENT"
---

## Goal

The administrator has the printable document of an issued invoice.

## Trigger

The invoice must be sent, printed or archived.

## Preconditions

The invoice is issued.

## Main Flow

1. The administrator downloads the document.
2. The product verifies the invoice fingerprint and returns the stored document.

## Alternative Flows

The stored document is missing: the product produces it again from the issued invoice, identical to the original, stores it and audits the regeneration.

## Failure Conditions

The invoice fails its fingerprint check: nothing is returned and an integrity failure is reported.

## Postconditions

The administrator has the document.
