---
id: FR-INT-105
type: functional-requirement
title: "Report payments through the public interface"
status: draft
derived-from:
  - "UC-INT-REPORT-PAYMENT-THROUGH-API"
  - "BR-INT-TOKEN-INHERITS-OWNER-ROLE"
verification:
  - id: "PAYMENT-REPORTED-THROUGH-API"
    scenario: "Given a token whose owner has financial rights, when an external application reads an invoice and sends a partial payment, the invoice's payment status and outstanding amount update; with a token whose owner lacks financial rights the request is refused."
uses-terms:
  - "TERM-PAYMENT-STATUS"
  - "TERM-INTEGRATION-REFERENCE"
provenance:
  source: ".private/research/help-center/articles/14042545177618.md, .private/research/help-center/articles/9723268717202.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let an external application with a valid token read invoices and their payments through the public interface and send the payment status of an invoice, including partial payments, and MUST update the invoice's payment status and outstanding amount accordingly. The product MUST refuse the request when the token owner lacks financial rights.

## Rationale

Companies keeping their books elsewhere can still bring payment status into the product.
