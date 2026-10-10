---
id: FR-INVOICE-007
type: functional-requirement
title: "Fingerprint issued invoices and refuse altered ones"
status: draft
derived-from:
  - "BR-INTEGRITY-VERIFICATION"
verification:
  - scenario-ref: "SB-TAMPERED-INVOICE-REFUSED"
  - scenario: "An issued invoice shows its fingerprint and the fingerprint appears in the issue audit event"
uses-terms:
  - "TERM-INVOICE-FINGERPRINT"
---

## Requirement

The product MUST compute a fingerprint for every issued invoice, show it, verify it whenever the invoice is shown or rendered, and refuse to show an invoice that fails verification.

## Rationale

Detects alteration that bypassed the product.
