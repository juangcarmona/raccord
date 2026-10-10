---
id: SB-RECTIFY-BY-DIFFERENCES
type: structured-behaviour
title: "Correcting an invoice by differences"
status: draft
illustrates:
  - "BR-RECTIFYING-INVOICE"
  - "UC-CORRECT-ISSUED-INVOICE"
given:
  - "Invoice F2026-00007 has been issued"
when: "The billing administrator corrects it by differences, removing one line of 200.00 before VAT at 21 percent"
then:
  - "The rectifying invoice is numbered R2026-00001 and references F2026-00007"
  - "It shows a taxable base of -200.00 and VAT of -42.00"
  - "F2026-00007 is unchanged"
uses-terms:
  - "TERM-RECTIFYING-INVOICE"
---

## Intent

Corrections leave the original intact and are traceable.

## Boundaries

None.
