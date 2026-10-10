---
id: SB-B2B-REJECTION-NEEDS-RECTIFICATION
type: structured-behaviour
title: "A rejected B2B invoice is corrected, not edited"
status: draft
illustrates:
  - "UC-EXCHANGE-B2B-E-INVOICE"
given:
  - "Invoice F2026-00012 was sent as a B2B e-invoice"
when: "The customer rejects it through the platform"
then:
  - "F2026-00012 shows the rejection and its reason"
  - "F2026-00012 stays unchanged"
  - "The billing administrator is prompted to issue a rectifying invoice"
uses-terms: []
---

## Intent

Immutability holds across e-invoicing.

## Boundaries

None.
