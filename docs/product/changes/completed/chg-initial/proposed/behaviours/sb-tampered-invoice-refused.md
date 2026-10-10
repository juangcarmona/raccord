---
id: SB-TAMPERED-INVOICE-REFUSED
type: structured-behaviour
title: "An altered issued invoice is not served"
status: draft
illustrates:
  - "BR-INTEGRITY-VERIFICATION"
given:
  - "An issued invoice's stored content was altered outside the product"
when: "Someone opens the invoice"
then:
  - "The invoice is not shown"
  - "A critical integrity failure is reported"
uses-terms: []
---

## Intent

Alteration is detected rather than propagated.

## Boundaries

None.
