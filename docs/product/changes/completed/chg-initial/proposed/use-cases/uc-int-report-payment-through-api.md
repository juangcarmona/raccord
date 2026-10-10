---
id: UC-INT-REPORT-PAYMENT-THROUGH-API
type: use-case
title: "Report payments through the public interface"
status: draft
primary-actor: "ACT-EXTERNAL-APPLICATION"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-INT-TOKEN-INHERITS-OWNER-ROLE"
uses-terms:
  - "TERM-PAYMENT-STATUS"
  - "TERM-INTEGRATION-REFERENCE"
provenance:
  source: ".private/research/help-center/articles/14042545177618.md, .private/research/help-center/articles/9723268717202.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Payment status recorded in an external system reaches the product.

## Trigger

An invoice is paid and recorded in an external system.

## Preconditions

Valid token with financial rights.

## Main Flow

1. The application reads invoices and their payments through the public interface.
2. The application sends the payment status of an invoice.
3. The product updates the invoice's payment status and outstanding amount.

## Alternative Flows

Partial payments are read and reported the same way.

## Failure Conditions

The token owner lacks financial rights: refused.

## Postconditions

The invoice shows the externally recorded payment.
